// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// CmovNotEqual32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class CmovNotEqual32 : X86Instruction
	{
		public override string AlternativeName { get { return "CmovNE32"; } }

		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x0F, 0x45 } );

		internal CmovNotEqual32()
			: base(1, 1)
		{
		}

		public override bool ThreeTwoAddressConversion { get { return false; } }

		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == DefaultResultCount);
			System.Diagnostics.Debug.Assert(node.OperandCount == DefaultOperandCount);

			emitter.Emit(LegacyOpcode, node.Result, node.Operand1);
		}

		// The following is used by the automated code generator.

		public override LegacyOpCode __legacyopcode { get { return LegacyOpcode; } }

		public override string __legacyOpcodeOperandOrder { get { return "r1"; } }
	}
}
