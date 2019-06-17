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
		/// Copies the next <see cref="sbyte"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="sbyte"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public sbyte ReadSByte() => ((sbyte*) _head++)[0];
		
		/// <summary>
		/// Copies the next <see cref="byte"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="byte"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public byte ReadByte() => _head++[0];
		
		/// <summary>
		/// Copies the next <see cref="bool"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="bool"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public bool ReadBool() => ((bool*) _head++)[0];

		/// <summary>
		/// Copies the next <see cref="short"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="short"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public short ReadShort()
		{
			var value = GetShort();
			_head += sizeof(short);
			return value;
		}

		/// <summary>
		/// Copies the next <see cref="ushort"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="ushort"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public ushort ReadUShort()
		{
			var value = GetUShort();
			_head += sizeof(ushort);
			return value;
		}

		/// <summary>
		/// Copies the next <see cref="char"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="char"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public char ReadChar()
		{
			var value = GetChar();
			_head += sizeof(char);
			return value;
		}

		/// <summary>
		/// Copies the next <see cref="int"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="int"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public int ReadInt()
		{
			var value = GetInt();
			_head += sizeof(int);
			return value;
		}

		/// <summary>
		/// Copies the next <see cref="uint"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="uint"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public uint ReadUInt()
		{
			var value = GetUInt();
			_head += sizeof(uint);
			return value;
		}

		/// <summary>
		/// Copies the next <see cref="long"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="long"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public long ReadLong()
		{
			var value = GetLong();
			_head += sizeof(long);
			return value;
		}

		/// <summary>
		/// Copies the next <see cref="ulong"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="ulong"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public ulong ReadULong()
		{
			var value = GetULong();
			_head += sizeof(ulong);
			return value;
		}

		/// <summary>
		/// Copies the next <see cref="decimal"/> of the stream to a stack variable, then advances the head.
		/// </summary>
		/// <returns>The <see cref="decimal"/> at the stream's head.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public decimal ReadDecimal()
		{
			var value = GetDecimal();
			_head += sizeof(decimal);
			return value;
		}
		
		/// <summary>
		/// Copies a subset of the stream into managed memory, then advances the head.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to retrieve.</param>
		/// <returns>The stream, starting from the head, as a <see cref="byte"/> array.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI, NotNull]
		public byte[] ReadByteArray(int lengthBytes)
		{
			var outArray = GetByteArray(lengthBytes);
			_head += lengthBytes;
			return outArray;
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
			value = ReadByteArray(lengthBytes);
			return this;
		}
	}
}