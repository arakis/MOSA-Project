﻿/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.RegisterAllocator
{

	public class LiveInterval : Interval
	{
		public VirtualRegister VirtualRegister { get; private set; }
		public int SpillCost { get; set; }

		public LiveInterval(VirtualRegister virtualRegister, int start, int end)
			: base(start, end)
		{
			this.VirtualRegister = virtualRegister;
			this.SpillCost = 0;
		}

	}

}

