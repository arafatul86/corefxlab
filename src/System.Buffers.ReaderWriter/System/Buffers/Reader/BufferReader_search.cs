﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public ref partial struct BufferReader<T> where T : unmanaged, IEquatable<T>
    {
        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiter"/>.
        /// </summary>
        /// <param name="span">The read data, if any.</param>
        /// <param name="delimiter">The delimiter to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> if found.</param>
        /// <returns>True if the <paramref name="delimiter"/> was found.</returns>
        public bool TryReadTo(out ReadOnlySpan<T> span, T delimiter, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = MemoryExtensions.IndexOf(remaining, delimiter);
            if (index != -1)
            {
                span = index == 0 ? default : remaining.Slice(0, index);
                Advance(index + (advancePastDelimiter ? 1 : 0));
                return true;
            }

            return TryReadToSlow(out span, delimiter, remaining.Length, advancePastDelimiter);
        }

        private bool TryReadToSlow(out ReadOnlySpan<T> span, T delimiter, int skip, bool advancePastDelimiter)
        {
            if (!TryReadToInternal(out ReadOnlySequence<T> sequence, delimiter, advancePastDelimiter, skip))
            {
                span = default;
                return false;
            }

            span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
            return true;
        }

        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiter"/>.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiter">The delimiter to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> if found.</param>
        /// <returns>True if the <paramref name="delimiter"/> was found.</returns>
        public bool TryReadTo(out ReadOnlySequence<T> sequence, T delimiter, bool advancePastDelimiter = true)
        {
            return TryReadToInternal(out sequence, delimiter, advancePastDelimiter);
        }

        public bool TryReadToInternal(out ReadOnlySequence<T> sequence, T delimiter, bool advancePastDelimiter, int skip = 0)
        {
            BufferReader<T> copy = this;
            if (skip > 0)
                Advance(skip);
            ReadOnlySpan<T> remaining = UnreadSpan;

            while (!End)
            {
                int index = remaining.IndexOf(delimiter);
                if (index != -1)
                {
                    // Found the delimiter. Move to it, slice, then move past it.
                    if (index > 0)
                    {
                        Advance(index);
                    }

                    sequence = Sequence.Slice(copy.Position, Position);
                    if (advancePastDelimiter)
                    {
                        Advance(1);
                    }
                    return true;
                }

                Advance(remaining.Length);
                remaining = CurrentSpan;
            }

            // Didn't find anything, reset our original state.
            this = copy;
            sequence = default;
            return false;
        }

        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiters"/>.
        /// </summary>
        /// <param name="span">The read data, if any.</param>
        /// <param name="delimiters">The delimiters to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the first found instance of any of the given <paramref name="delimiters"/>.</param>
        /// <returns>True if any of the the <paramref name="delimiters"/> were found.</returns>
        public bool TryReadToAny(out ReadOnlySpan<T> span, ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = remaining.IndexOfAny(delimiters);
            if (index != -1)
            {
                span = remaining.Slice(0, index);
                Advance(index + (advancePastDelimiter ? 1 : 0));
                return true;
            }

            return TryReadToAnySlow(out span, delimiters, remaining.Length, advancePastDelimiter);
        }

        private bool TryReadToAnySlow(out ReadOnlySpan<T> span, ReadOnlySpan<T> delimiters, int skip, bool advancePastDelimiter)
        {
            if (!TryReadToAnyInternal(out ReadOnlySequence<T> sequence, delimiters, advancePastDelimiter, skip))
            {
                span = default;
                return false;
            }

            span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
            return true;
        }

        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiters"/>.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiters">The delimiters to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the first found instance of any of the given <paramref name="delimiters"/>.</param>
        /// <returns>True if any of the the <paramref name="delimiters"/> were found.</returns>
        public bool TryReadToAny(out ReadOnlySequence<T> sequence, ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
        {
            return TryReadToAnyInternal(out sequence, delimiters, advancePastDelimiter);
        }

        private bool TryReadToAnyInternal(out ReadOnlySequence<T> sequence, ReadOnlySpan<T> delimiters, bool advancePastDelimiter, int skip = 0)
        {
            BufferReader<T> copy = this;
            if (skip > 0)
                Advance(skip);
            ReadOnlySpan<T> remaining = CurrentSpanIndex == 0 ? CurrentSpan : UnreadSpan;

            while (!End)
            {
                int index = delimiters.Length == 2
                    ? remaining.IndexOfAny(delimiters[0], delimiters[1])
                    : remaining.IndexOfAny(delimiters);

                if (index != -1)
                {
                    // Found one of the delimiters. Move to it, slice, then move past it.
                    if (index > 0)
                    {
                        Advance(index);
                    }

                    sequence = Sequence.Slice(copy.Position, Position);
                    if (advancePastDelimiter)
                    {
                        Advance(1);
                    }
                    return true;
                }

                Advance(remaining.Length);
                remaining = CurrentSpan;
            }

            // Didn't find anything, reset our original state.
            this = copy;
            sequence = default;
            return false;
        }

        /// <summary>
        /// Try to read data until the given <paramref name="delimiter"/> sequence.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiter">The multi (T) delimiter.</param>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> sequence if found.</param>
        /// <returns>True if the <paramref name="delimiter"/> was found.</returns>
        public unsafe bool TryReadTo(out ReadOnlySequence<T> sequence, ReadOnlySpan<T> delimiter, bool advancePastDelimiter = true)
        {
            if (delimiter.Length == 0)
            {
                sequence = default;
                return true;
            }

            BufferReader<T> copy = this;

            Span<T> peekBuffer;
            if (delimiter.Length * sizeof(T) < 512)
            {
                T* t = stackalloc T[delimiter.Length];
                peekBuffer = new Span<T>(t, delimiter.Length);
            }
            else
            {
                peekBuffer = new Span<T>(new T[delimiter.Length]);
            }

            while (!End)
            {
                if (!TryReadTo(out sequence, delimiter[0], advancePastDelimiter: false))
                {
                    this = copy;
                    return false;
                }

                if (delimiter.Length == 1)
                {
                    return true;
                }

                ReadOnlySpan<T> next = Peek(peekBuffer);
                if (next.SequenceEqual(delimiter))
                {
                    if (advancePastDelimiter)
                    {
                        Advance(delimiter.Length);
                    }
                    return true;
                }
                else
                {
                    Advance(1);
                }
            }

            this = copy;
            sequence = default;
            return false;
        }

        /// <summary>
        /// Skip until the given <paramref name="delimiter"/>, if found.
        /// </summary>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> if found.</param>
        /// <returns>True if the given <paramref name="delimiter"/> was found.</returns>
        public bool TrySkipTo(T delimiter, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = remaining.IndexOf(delimiter);
            if (index != -1)
            {
                Advance(index);
                return true;
            }

            return TryReadToInternal(out _, delimiter, advancePastDelimiter);
        }

        /// <summary>
        /// Skip until any of the given <paramref name="delimiters"/>, if found.
        /// </summary>
        /// <param name="advancePastDelimiter">True to move past the first found instance of any of the given <paramref name="delimiters"/>.</param>
        /// <returns>True if any of the given <paramref name="delimiters"/> was found.</returns>
        public bool TrySkipToAny(ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = remaining.IndexOfAny(delimiters);
            if (index != -1)
            {
                Advance(index);
                return true;
            }

            return TryReadToAnyInternal(out _, delimiters, advancePastDelimiter);
        }

        /// <summary>
        /// Skip consecutive instances of the given <paramref name="value"/>.
        /// </summary>
        /// <returns>Count of skipped <typeparamref name="T"/> values.</returns>
        public long SkipPast(T value)
        {
            long start = Consumed;

            do
            {
                // Skip all matches in the current span
                int i;
                for (i = CurrentSpanIndex; i < CurrentSpan.Length && CurrentSpan[i].Equals(value); i++)
                {
                }

                int advanced = i - CurrentSpanIndex;
                if (advanced == 0)
                {
                    // Didn't skip at all in this span, exit.
                    break;
                }

                Advance(advanced);

                // If we're at postion 0 after advancing and not at the End,
                // we're in a new span and should continue the loop.
            } while (CurrentSpanIndex == 0 && !End);

            return Consumed - start;
        }

        /// <summary>
        /// Skip consecutive instances of any of the given <paramref name="values"/>.
        /// </summary>
        /// <returns>Count of skipped <typeparamref name="T"/> values.</returns>
        public long SkipPastAny(ReadOnlySpan<T> values)
        {
            long start = Consumed;

            do
            {
                // Skip all matches in the current span
                int i;
                for (i = CurrentSpanIndex; i < CurrentSpan.Length && values.IndexOf(CurrentSpan[i]) != -1; i++)
                {
                }

                int advanced = i - CurrentSpanIndex;
                if (advanced == 0)
                {
                    // Didn't skip at all in this span, exit.
                    break;
                }

                Advance(advanced);

                // If we're at postion 0 after advancing and not at the End,
                // we're in a new span and should continue the loop.
            } while (CurrentSpanIndex == 0 && !End);

            return Consumed - start;
        }

        /// <summary>
        /// Skip consecutive instances of any of the given values.
        /// </summary>
        /// <returns>Count of skipped <typeparamref name="T"/> values.</returns>
        public long SkipPastAny(T value0, T value1, T value2, T value3)
        {
            long start = Consumed;

            do
            {
                // Skip all matches in the current span
                int i;
                for (i = CurrentSpanIndex; i < CurrentSpan.Length; i++)
                {
                    T value = CurrentSpan[i];
                    if (!value.Equals(value0) && !value.Equals(value1) && !value.Equals(value2) && !value.Equals(value3))
                    {
                        break;
                    }
                }

                int advanced = i - CurrentSpanIndex;
                if (advanced == 0)
                {
                    // Didn't skip at all in this span, exit.
                    break;
                }

                Advance(advanced);

                // If we're at postion 0 after advancing and not at the End,
                // we're in a new span and should continue the loop.
            } while (CurrentSpanIndex == 0 && !End);

            return Consumed - start;
        }

        /// <summary>
        /// Skip consecutive instances of any of the given values.
        /// </summary>
        /// <returns>Count of skipped <typeparamref name="T"/> values.</returns>
        public long SkipPastAny(T value0, T value1, T value2)
        {
            long start = Consumed;

            do
            {
                // Skip all matches in the current span
                int i;
                for (i = CurrentSpanIndex; i < CurrentSpan.Length; i++)
                {
                    T value = CurrentSpan[i];
                    if (!value.Equals(value0) && !value.Equals(value1) && !value.Equals(value2))
                    {
                        break;
                    }
                }

                int advanced = i - CurrentSpanIndex;
                if (advanced == 0)
                {
                    // Didn't skip at all in this span, exit.
                    break;
                }

                Advance(advanced);

                // If we're at postion 0 after advancing and not at the End,
                // we're in a new span and should continue the loop.
            } while (CurrentSpanIndex == 0 && !End);

            return Consumed - start;
        }

        /// <summary>
        /// Skip consecutive instances of any of the given values.
        /// </summary>
        /// <returns>Count of skipped <typeparamref name="T"/> values.</returns>
        public long SkipPastAny(T value0, T value1)
        {
            long start = Consumed;

            do
            {
                // Skip all matches in the current span
                int i;
                for (i = CurrentSpanIndex; i < CurrentSpan.Length; i++)
                {
                    T value = CurrentSpan[i];
                    if (!value.Equals(value0) && !value.Equals(value1))
                    {
                        break;
                    }
                }

                int advanced = i - CurrentSpanIndex;
                if (advanced == 0)
                {
                    // Didn't skip at all in this span, exit.
                    break;
                }

                Advance(advanced);

                // If we're at postion 0 after advancing and not at the End,
                // we're in a new span and should continue the loop.
            } while (CurrentSpanIndex == 0 && !End);

            return Consumed - start;
        }

        /// <summary>
        /// Check to see if the given <paramref name="next"/> value is next.
        /// </summary>
        /// <param name="advancePast">Move past the <paramref name="next"/> value if found.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNext(T next, bool advancePast = false)
        {
            if (End)
                return false;

            ReadOnlySpan<T> unread = UnreadSpan;
            if (unread[0].Equals(next))
            {
                if (advancePast)
                {
                    Advance(1);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check to see if the given <paramref name="next"/> values are next.
        /// </summary>
        /// <param name="advancePast">Move past the <paramref name="next"/> values if found.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNext(ReadOnlySpan<T> next, bool advancePast = false)
        {
            ReadOnlySpan<T> unread = UnreadSpan;
            if (unread.StartsWith(next))
            {
                if (advancePast)
                {
                    Advance(next.Length);
                }
                return true;
            }
            return unread.Length < next.Length && IsNextSlow(next, advancePast);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe bool IsNextSlow(ReadOnlySpan<T> next, bool advancePast)
        {
            // Call PeekSlow directly since we know there isn't enough space.
            Debug.Assert(UnreadSpan.Length < next.Length);

            T* t = stackalloc T[next.Length];
            ReadOnlySpan<T> peek = PeekSlow(new Span<T>(t, next.Length));

            if (next.SequenceEqual(peek))
            {
                if (advancePast)
                {
                    Advance(next.Length);
                }
                return true;
            }

            return false;
        }
    }
}
