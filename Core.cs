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
		public BinaryStream(int bufferSize)
		{
			_memory = (byte*) Marshal.AllocHGlobal(bufferSize);
			_head = _memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		private void ReleaseUnmanagedResources()
		{
			Marshal.Release((IntPtr) _memory);
			_memory = null;
			_head = null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public void Dispose()
		{
			ReleaseUnmanagedResources();
		}
	}
}
