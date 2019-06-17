/**
 * Copyright 2019 Max Pixel
 *
 * This file is part of FastestBinaryStream by Max Pixel.
 *
 * FastestBinaryStream is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser
 * General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * FastestBinaryStream is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
 * implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License
 * for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License along with FastestBinaryStream.  If not, see
 * <https://www.gnu.org/licenses/>.
 */

using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace FastestBinaryStream
{
	public unsafe ref partial struct BinaryStream
	{
		/// <summary>
		/// Writes a <see cref="short"/> to the stream in big-endian byte order, and advances the head.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream WriteBig(short value)
		{
			*_head++ = (byte) (value >> 8);
			*_head++ = (byte) value;
			return this;
		}

		/// <summary>
		/// Writes a <see cref="ushort"/> to the stream in big-endian byte order, and advances the head.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream WriteBig(ushort value)
		{
			*_head++ = (byte) (value >> 8);
			*_head++ = (byte) value;
			return this;
		}

		/// <summary>
		/// Writes a <see cref="char"/> to the stream in big-endian byte order, and advances the head.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream WriteBig(char value)
		{
			*_head++ = (byte) (value >> 8);
			*_head++ = (byte) value;
			return this;
		}

		/// <summary>
		/// Writes an <see cref="int"/> to the stream in big-endian byte order, and advances the head.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream WriteBig(int value)
		{
			*_head++ = (byte) (value >> 24);
			*_head++ = (byte) (value >> 16);
			*_head++ = (byte) (value >> 8);
			*_head++ = (byte) value;
			return this;
		}

		/// <summary>
		/// Writes a <see cref="uint"/> to the stream in big-endian byte order, and advances the head.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream WriteBig(uint value)
		{
			*_head++ = (byte) (value >> 24);
			*_head++ = (byte) (value >> 16);
			*_head++ = (byte) (value >> 8);
			*_head++ = (byte) value;
			return this;
		}

		/// <summary>
		/// Writes a <see cref="long"/> to the stream in big-endian byte order, and advances the head.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream WriteBig(long value)
		{
			*_head++ = (byte) (value >> 56);
			*_head++ = (byte) (value >> 48);
			*_head++ = (byte) (value >> 40);
			*_head++ = (byte) (value >> 32);
			*_head++ = (byte) (value >> 24);
			*_head++ = (byte) (value >> 16);
			*_head++ = (byte) (value >> 8);
			*_head++ = (byte) value;
			return this;
		}

		/// <summary>
		/// Writes a <see cref="ulong"/> to the stream in big-endian byte order, and advances the head.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream WriteBig(ulong value)
		{
			*_head++ = (byte) (value >> 56);
			*_head++ = (byte) (value >> 48);
			*_head++ = (byte) (value >> 40);
			*_head++ = (byte) (value >> 32);
			*_head++ = (byte) (value >> 24);
			*_head++ = (byte) (value >> 16);
			*_head++ = (byte) (value >> 8);
			*_head++ = (byte) value;
			return this;
		}
	}
}