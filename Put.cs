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

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace FastestBinaryStream
{
	public unsafe ref partial struct BinaryStream
	{
		/// <summary>Writes a <see cref="T"/> to the stream without advancing the head.</summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Put<T>(T value) where T : unmanaged
		{
			*(T*) _head = value;
			return this;
		}

		/// <summary>Writes a <see cref="string"/> to the stream without advancing the head.</summary>
		/// <param name="value">The value to write.</param>
		/// <param name="stringSizeBytes">Returns the size of the string in bytes.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Put([NotNull] string value, out int stringSizeBytes)
		{
			stringSizeBytes = sizeof(char) * value.Length;
			Put(value, stringSizeBytes);
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Put([NotNull] string value, int stringSizeBytes)
		{
			fixed (char* chars = value)
				Buffer.MemoryCopy(chars, _head, stringSizeBytes, stringSizeBytes);
		}

		/// <summary>Writes a <see cref="string"/> to the stream without advancing the head.</summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Put([NotNull] string value) => Put(value, out _);

		/// <summary>Writes an array of <see cref="byte"/>s to the stream without advancing the head.</summary>
		/// <param name="value">The value to write.</param>
		/// <param name="lengthBytes">Returns the size of the array in bytes.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Put([NotNull] byte[] value, out int lengthBytes)
		{
			lengthBytes = value.Length;
			fixed (byte* source = value)
			{
				Buffer.MemoryCopy(source, _head, lengthBytes, lengthBytes);
			}

			return this;
		}

		/// <summary>Writes an array of <see cref="byte"/>s to the stream without advancing the head.</summary>
		/// <param name="value">The value to write.</param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Put([NotNull] byte[] value) => Put(value, out _);
	}
}