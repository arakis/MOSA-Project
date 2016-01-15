﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Reflection;
using Mosa.Runtime;

namespace System
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal LinkedList<RuntimeType> typeList = new LinkedList<RuntimeType>();
		internal LinkedList<RuntimeTypeHandle> typeHandles = new LinkedList<RuntimeTypeHandle>();
		internal LinkedList<RuntimeTypeInfo> typeInfoList = null;
		internal LinkedList<CustomAttributeData> customAttributesData = null;

		private string fullName;

		public override IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				if (customAttributesData == null)
				{
					// Custom Attributes Data - Lazy load
					customAttributesData = new LinkedList<CustomAttributeData>();
					if (assemblyStruct->CustomAttributes != null)
					{
						var customAttributesTablePtr = this.assemblyStruct->CustomAttributes;
						var customAttributesCount = customAttributesTablePtr[0];
						customAttributesTablePtr++;
						for (uint i = 0; i < customAttributesCount; i++)
						{
							var cad = new RuntimeCustomAttributeData((MetadataCAStruct*)customAttributesTablePtr[i]);
							customAttributesData.AddLast(cad);
						}
					}
				}

				return this.customAttributesData;
			}
		}

		public override IEnumerable<TypeInfo> DefinedTypes
		{
			get
			{
				if (this.typeInfoList == null)
				{
					// Type Info - Lazy load
					this.typeInfoList = new LinkedList<RuntimeTypeInfo>();
					foreach (RuntimeType type in this.typeList)
						this.typeInfoList.AddLast(new RuntimeTypeInfo(type, this));
				}

				var types = new LinkedList<TypeInfo>();
				foreach (var type in this.typeInfoList)
					types.AddLast(type);
				return types;
			}
		}

		public override string FullName
		{
			get { return this.fullName; }
		}

		public override IEnumerable<Type> ExportedTypes
		{
			get
			{
				var types = new LinkedList<Type>();
				foreach (RuntimeType type in this.typeList)
				{
					if ((type.attributes & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
						continue;
					types.AddLast(type);
				}
				return types;
			}
		}

		internal RuntimeAssembly(uint* pointer)
		{
			this.assemblyStruct = (MetadataAssemblyStruct*)pointer;
			this.fullName = Mosa.Runtime.Internal.InitializeMetadataString(this.assemblyStruct->Name);

			uint typeCount = (*this.assemblyStruct).NumberOfTypes;
			for (uint i = 0; i < typeCount; i++)
			{
				var handle = new RuntimeTypeHandle();
				((uint**)&handle)[0] = (uint*)MetadataAssemblyStruct.GetTypeDefinitionAddress(assemblyStruct, i);

				if (this.typeHandles.Contains(handle))
					continue;

				this.ProcessType(handle);
			}
		}

		internal RuntimeType ProcessType(RuntimeTypeHandle handle)
		{
			this.typeHandles.AddLast(handle);
			var type = new RuntimeType(handle);
			this.typeList.AddLast(type);
			return type;
		}
	}
}