// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// CompareIntBranch64
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class CompareIntBranch64 : BaseIRInstruction
	{
		public CompareIntBranch64()
			: base(0, 2)
		{
		}

		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }
	}
}
