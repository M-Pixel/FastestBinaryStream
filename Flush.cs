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
		/// <param name="value">
		/// Returns the stream, from position 0 to the head, as a <see cref="byte"/> array.
		/// </param>
		/// <param name="lengthBytes">Returns the quantity of bytes to be flushed.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream FlushByteArray(out byte[] value, out int lengthBytes)
		{
			lengthBytes = (int) (_head - _memory);
			Rewind();
			return ReadByteArray(lengthBytes, out value);
		}

		/// <param name="value">
		/// Returns the stream, from position 0 to the head, as a <see cref="byte"/> array.
		/// </param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream FlushByteArray(out byte[] value) => FlushByteArray(out value, out _);
	}
}