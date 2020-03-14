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
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace FastestBinaryStream
{
	public unsafe ref partial struct BinaryStream
	{
		private byte* _memory;

		private byte* _head;
		
		
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream(int bufferSize) : this(Marshal.AllocHGlobal(bufferSize))
		{
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream(IntPtr memory) : this((byte*) memory)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream(byte* memory)
		{
			_memory = memory;
			_head = memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		private void FreeUnmanagedResources()
		{
			Marshal.FreeHGlobal((IntPtr) _memory);
			_memory = null;
			_head = null;
		}

		/// <summary>
		/// If the <c>int bufferSize</c> constructor is used, then Dispose must be called before the instance is
		/// de-referenced, otherwise its memory will never be de-allocated.  Alternately, <see cref="ReturnIntPtr"/> may
		/// be called, in which case the returned <see cref="IntPtr"/> would need to be passed to
		/// <see cref="Marshal.FreeHGlobal"/> before it is de-referenced, in order to prevent a memory leak.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public void Dispose()
		{
			FreeUnmanagedResources();
		}

		/// <summary>
		/// If the <c>int bufferSize</c> constructor is used, then Dispose must be called before the instance is
		/// de-referenced, otherwise its memory will never be de-allocated.
		///
		/// This version of Dispose returns a value that it is given, to facilitate using this class in bracket-less
		/// lambda expressions.  For example,
		/// <code>
		/// byte[] SomePureMethod(SomethingWorthSerializing x) => new BinaryStream(64)
		///     .Write(x.A)
		///     .Write(x.B)
		///     .FlushBase64String(out var returnValue)
		///     .DisposeAndReturn(returnValue);
		/// </code>
		/// </summary>
		/// <param name="valueToReturn">Value that you want to be returned by this method.</param>
		/// <returns><see cref="valueToReturn"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public T DisposeAndReturn<T>(T valueToReturn)
		{
			FreeUnmanagedResources();
			return valueToReturn;
		}

		/// <summary>
		/// This method returns the value that it is given, to facilitate using this class in bracket-less lambda
		/// expressions.  For example,
		/// <code>
		/// string SomePureMethod(IntPtr x, int index) => new BinaryStream(x)
		///     .Skip(index * 2)
		///     .ReadByte(out var offset)
		///     .ReadByte(out var length)
		///     .SetHeadByOffset(offset)
		///     .ReadBase64String(length, out var returnValue)
		///     .Return(returnValue);
		/// </code>
		/// Note: If using the <c>int bufferSize</c> constructor, you must use <see cref="DisposeAndReturn{T}"/>
		/// instead, otherwise the memory will never be released.
		/// </summary>
		/// <param name="valueToReturn">Value that you want to be returned by this method.</param>
		/// <returns><see cref="valueToReturn"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public T Return<T>(T valueToReturn) => valueToReturn;


		/// Set the position of the head relative to the buffer.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream SetHeadByOffset(int byteIndex) => SetHeadByOffset((long) byteIndex);

		/// Set the position of the head relative to the buffer.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream SetHeadByOffset(long byteIndex)
		{
			_head = _memory + byteIndex;
			return this;
		}

		/// Set the head to a precise location in memory.  You're responsible for ensuring that the address you give
		/// falls within the buffer.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream SetHeadByAddress(IntPtr head) => SetHeadByAddress((byte*) head);

		/// Set the head to a precise location in memory.  You're responsible for ensuring that the address you give
		/// falls within the buffer.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream SetHeadByAddress(byte* address)
		{
			_head = address;
			return this;
		}

		/// Change the position of the head relative to its current position.  To move it backwards, use a negative
		/// number of bytes to skip.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Skip(int bytesToSkip) => Skip((long) bytesToSkip);
		
		/// Change the position of the head relative to its current position.  To move it backwards, use a negative
		/// number of bytes to skip.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Skip(long bytesToSkip)
		{
			_head += bytesToSkip;
			return this;
		}

		/// Set the head to the beginning of the buffer.  Equivalent to SetHeadByOffset(0).
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Rewind()
		{
			_head = _memory;
			return this;
		}


		/// Get the address of the buffer.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReportMemoryAddress(out IntPtr address)
		{
			address = (IntPtr) _memory;
			return this;
		}

		/// Get the address of the buffer.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReportMemoryAddress(out byte* address)
		{
			address = _memory;
			return this;
		}

		/// Get the address of the head.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReportHeadAddress(out IntPtr address)
		{
			address = (IntPtr) _head;
			return this;
		}

		/// Get the address of the head.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReportHeadAddress(out byte* address)
		{
			address = _head;
			return this;
		}

		/// Get the offset of the head from the base of the buffer.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReportHeadOffset(out long offset)
		{
			offset = _head - _memory;
			return this;
		}

		/// Create a copy of this Stream object in its current state.  Does not copy the underlying memory.  This is an
		/// easy way to "bookmark" a place in the buffer for later use.
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream Fork(out BinaryStream fork)
		{
			fork = new BinaryStream(_memory) {_head = _head};
			return this;
		}
	}
}
