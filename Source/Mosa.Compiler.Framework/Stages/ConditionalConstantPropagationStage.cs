﻿/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class ConditionalConstantPropagationStage : BaseMethodCompilerStage
	{
		protected SectionTrace trace;

		protected int conditionalConstantPropagation = 0;

		protected override void Setup()
		{
			trace = CreateTrace();
		}

		protected override void Run()
		{
			var analysis = new ConditionalConstantPropagation(BasicBlocks, InstructionSet, trace);

			var deadBlocks = analysis.GetDeadBlocked();
			var constants = analysis.GetIntegerConstants();

			ReplaceVirtualRegistersWithConstants(constants);

			UpdateCounter("ConditionalConstantPropagation.ConstantVariableCount", constants.Count);
			UpdateCounter("ConditionalConstantPropagation.ConstantVariableUse", conditionalConstantPropagation);
		}

		protected void ReplaceVirtualRegistersWithConstants(List<Tuple<Operand, ulong>> constantVirtualRegisters)
		{
			foreach (var value in constantVirtualRegisters)
			{
				ReplaceVirtualRegisterWithConstant(value.Item1, value.Item2);
			}
		}

		protected void ReplaceVirtualRegisterWithConstant(Operand target, ulong value)
		{
			if (trace.Active) trace.Log(target.ToString() + " = " + value.ToString() + " Uses: " + target.Uses.Count.ToString());

			if (target.Uses.Count == 0)
				return;

			var constant = Operand.CreateConstant(target.Type, value);

			// for each statement T that uses operand, substituted c in statement T
			foreach (int index in target.Uses.ToArray())
			{
				var context = new Context(InstructionSet, index);

				Debug.Assert(context.Instruction != IRInstruction.AddressOf);

				for (int i = 0; i < context.OperandCount; i++)
				{
					var operand = context.GetOperand(i);

					if (operand != target)
						continue;

					if (trace.Active) trace.Log("*** ConditionalConstantPropagation");
					if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
					context.SetOperand(i, constant);
					conditionalConstantPropagation++;
					if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				}
			}
		}
	}
}