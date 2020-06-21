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
		/// Copies the next <see cref="short"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <param name="value">Returns the <c>T</c> at the stream's head.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Read<T>(out T value) where T : unmanaged
		{
			Get(out value);
			_head += sizeof(T);
			return this;
		}

		/// <summary>
		/// Copies a subset of the stream into managed memory, then advances the head.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to retrieve.</param>
		/// <param name="value">Returns the stream, starting from the head, as a <see cref="byte"/> array.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReadByteArray(int lengthBytes, out byte[] value)
		{
			GetByteArray(lengthBytes, out value);
			_head += lengthBytes;
			return this;
		}
	}
}