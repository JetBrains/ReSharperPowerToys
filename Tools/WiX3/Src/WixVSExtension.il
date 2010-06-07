
//  Microsoft (R) .NET Framework IL Disassembler.  Version 2.0.50727.42
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v1.1.4322
.assembly extern wix
{
  .publickeytoken = (CE 35 F7 6F CD A8 2B AD )                         // .5.o..+.
  .ver 3:0:2420:0
}
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 1:0:5000:0
}
.assembly extern System.Xml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 1:0:5000:0
}
.assembly WixVSExtension
{
  .custom instance void [mscorlib]System.CLSCompliantAttribute::.ctor(bool) = {bool(true)}
  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string)
           = {string('WiX Toolset VS Extension')}
  .custom instance void [wix]Microsoft.Tools.WindowsInstallerXml.AssemblyDefaultWixExtensionAttribute::.ctor(class [mscorlib]System.Type)
           = {type(Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension)}
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool)
           = {bool(false)}
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string)
           = {string('')}
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string)
           = {string('Copyright © Microsoft Corporation. All rights reserved.')}
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string)
           = {string('Windows Installer XML Toolset')}
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string)
           = {string('Microsoft Corporation')}
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string)
           = {string('')}
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string)
           = {string('Windows Installer XML Toolset Visual Studio Extension')}
  .hash algorithm 0x00008004
  .ver 3:0:2420:0
}
.mresource public Microsoft.Tools.WindowsInstallerXml.Extensions.Xsd.vs.xsd
{
  // Offset: 0x00000000 Length: 0x000009A4
  // WARNING: managed resource file Microsoft.Tools.WindowsInstallerXml.Extensions.Xsd.vs.xsd created
}
.mresource public Microsoft.Tools.WindowsInstallerXml.Extensions.Data.vs.wixlib
{
  // Offset: 0x000009A8 Length: 0x00006901
  // WARNING: managed resource file Microsoft.Tools.WindowsInstallerXml.Extensions.Data.vs.wixlib created
}
.mresource public Microsoft.Tools.WindowsInstallerXml.Extensions.Data.tables.xml
{
  // Offset: 0x000072B0 Length: 0x000014E6
  // WARNING: managed resource file Microsoft.Tools.WindowsInstallerXml.Extensions.Data.tables.xml created
}
.module WixVSExtension.dll
// MVID: {01EEA1C3-E3F6-4AED-A9D6-51C513F7E604}
.imagebase 0x00400000
.file alignment 0x00001000
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x003A0000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi sealed beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler
       extends [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension
{
  .field private class [System.Xml]System.Xml.Schema.XmlSchema schema
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       28 (0x1c)
    .maxstack  3
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::.ctor()
    IL_0006:  ldarg.0
    IL_0007:  call       class [mscorlib]System.Reflection.Assembly [mscorlib]System.Reflection.Assembly::GetExecutingAssembly()
    IL_000c:  ldstr      "Microsoft.Tools.WindowsInstallerXml.Extensions.Xsd"
    + ".vs.xsd"
    IL_0011:  call       class [System.Xml]System.Xml.Schema.XmlSchema [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::LoadXmlSchemaHelper(class [mscorlib]System.Reflection.Assembly,
                                                                                                                                                       string)
    IL_0016:  stfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
    IL_001b:  ret
  } // end of method VSCompiler::.ctor

  .method public hidebysig specialname virtual 
          instance class [System.Xml]System.Xml.Schema.XmlSchema 
          get_Schema() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
    IL_0006:  ret
  } // end of method VSCompiler::get_Schema

  .method public hidebysig virtual instance void 
          ParseElement(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection sourceLineNumbers,
                       class [System.Xml]System.Xml.XmlElement parentElement,
                       class [System.Xml]System.Xml.XmlElement element,
                       string[] contextValues) cil managed
  {
    .param [4]
    .custom instance void [mscorlib]System.ParamArrayAttribute::.ctor() = {}
    // Code size       236 (0xec)
    .maxstack  5
    .locals init (string V_0)
    IL_0000:  ldstr      "File"
    IL_0005:  ldstr      "Fragment"
    IL_000a:  ldstr      "Product"
    IL_000f:  leave.s    IL_0011

    IL_0011:  ldarg.2
    IL_0012:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
    IL_0017:  dup
    IL_0018:  stloc.0
    IL_0019:  brfalse    IL_00e0

    IL_001e:  ldloc.0
    IL_001f:  call       string [mscorlib]System.String::IsInterned(string)
    IL_0024:  stloc.0
    IL_0025:  ldloc.0
    IL_0026:  ldstr      "File"
    IL_002b:  beq.s      IL_0042

    IL_002d:  ldloc.0
    IL_002e:  ldstr      "Fragment"
    IL_0033:  beq.s      IL_0095

    IL_0035:  ldloc.0
    IL_0036:  ldstr      "Product"
    IL_003b:  beq.s      IL_0095

    IL_003d:  br         IL_00e0

    IL_0042:  ldstr      "HelpCollection"
    IL_0047:  ldstr      "HelpFile"
    IL_004c:  leave.s    IL_004e

    IL_004e:  ldarg.3
    IL_004f:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
    IL_0054:  dup
    IL_0055:  stloc.0
    IL_0056:  brfalse.s  IL_0089

    IL_0058:  ldloc.0
    IL_0059:  call       string [mscorlib]System.String::IsInterned(string)
    IL_005e:  stloc.0
    IL_005f:  ldloc.0
    IL_0060:  ldstr      "HelpCollection"
    IL_0065:  beq.s      IL_0071

    IL_0067:  ldloc.0
    IL_0068:  ldstr      "HelpFile"
    IL_006d:  beq.s      IL_007d

    IL_006f:  br.s       IL_0089

    IL_0071:  ldarg.0
    IL_0072:  ldarg.3
    IL_0073:  ldarg.s    contextValues
    IL_0075:  ldc.i4.0
    IL_0076:  ldelem.ref
    IL_0077:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParseHelpCollectionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                             string)
    IL_007c:  ret

    IL_007d:  ldarg.0
    IL_007e:  ldarg.3
    IL_007f:  ldarg.s    contextValues
    IL_0081:  ldc.i4.0
    IL_0082:  ldelem.ref
    IL_0083:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParseHelpFileElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                       string)
    IL_0088:  ret

    IL_0089:  ldarg.0
    IL_008a:  ldarg.1
    IL_008b:  ldarg.2
    IL_008c:  ldarg.3
    IL_008d:  ldarg.s    contextValues
    IL_008f:  call       instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::ParseElement(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                class [System.Xml]System.Xml.XmlElement,
                                                                                                                class [System.Xml]System.Xml.XmlElement,
                                                                                                                string[])
    IL_0094:  ret

    IL_0095:  ldstr      "HelpCollectionRef"
    IL_009a:  ldstr      "HelpFilter"
    IL_009f:  leave.s    IL_00a1

    IL_00a1:  ldarg.3
    IL_00a2:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
    IL_00a7:  dup
    IL_00a8:  stloc.0
    IL_00a9:  brfalse.s  IL_00d4

    IL_00ab:  ldloc.0
    IL_00ac:  call       string [mscorlib]System.String::IsInterned(string)
    IL_00b1:  stloc.0
    IL_00b2:  ldloc.0
    IL_00b3:  ldstr      "HelpCollectionRef"
    IL_00b8:  beq.s      IL_00c4

    IL_00ba:  ldloc.0
    IL_00bb:  ldstr      "HelpFilter"
    IL_00c0:  beq.s      IL_00cc

    IL_00c2:  br.s       IL_00d4

    IL_00c4:  ldarg.0
    IL_00c5:  ldarg.3
    IL_00c6:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParseHelpCollectionRefElement(class [System.Xml]System.Xml.XmlNode)
    IL_00cb:  ret

    IL_00cc:  ldarg.0
    IL_00cd:  ldarg.3
    IL_00ce:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParseHelpFilterElement(class [System.Xml]System.Xml.XmlNode)
    IL_00d3:  ret

    IL_00d4:  ldarg.0
    IL_00d5:  ldarg.1
    IL_00d6:  ldarg.2
    IL_00d7:  ldarg.3
    IL_00d8:  ldarg.s    contextValues
    IL_00da:  call       instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::ParseElement(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                class [System.Xml]System.Xml.XmlElement,
                                                                                                                class [System.Xml]System.Xml.XmlElement,
                                                                                                                string[])
    IL_00df:  ret

    IL_00e0:  ldarg.0
    IL_00e1:  ldarg.1
    IL_00e2:  ldarg.2
    IL_00e3:  ldarg.3
    IL_00e4:  ldarg.s    contextValues
    IL_00e6:  call       instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::ParseElement(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                class [System.Xml]System.Xml.XmlElement,
                                                                                                                class [System.Xml]System.Xml.XmlElement,
                                                                                                                string[])
    IL_00eb:  ret
  } // end of method VSCompiler::ParseElement

  .method private hidebysig instance void 
          ParseHelpCollectionRefElement(class [System.Xml]System.Xml.XmlNode node) cil managed
  {
    // Code size       426 (0x1aa)
    .maxstack  6
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection V_0,
             string V_1,
             class [System.Xml]System.Xml.XmlAttribute V_2,
             class [System.Xml]System.Xml.XmlNode V_3,
             class [mscorlib]System.Collections.IEnumerator V_4,
             string V_5,
             string[] V_6,
             class [mscorlib]System.IDisposable V_7)
    IL_0000:  ldarg.1
    IL_0001:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Preprocessor::GetSourceLineNumbers(class [System.Xml]System.Xml.XmlNode)
    IL_0006:  stloc.0
    IL_0007:  ldnull
    IL_0008:  stloc.1
    IL_0009:  ldarg.1
    IL_000a:  callvirt   instance class [System.Xml]System.Xml.XmlAttributeCollection [System.Xml]System.Xml.XmlNode::get_Attributes()
    IL_000f:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNamedNodeMap::GetEnumerator()
    IL_0014:  stloc.s    V_4
    .try
    {
      IL_0016:  br         IL_00bd

      IL_001b:  ldloc.s    V_4
      IL_001d:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0022:  castclass  [System.Xml]System.Xml.XmlAttribute
      IL_0027:  stloc.2
      IL_0028:  ldloc.2
      IL_0029:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_002e:  callvirt   instance int32 [mscorlib]System.String::get_Length()
      IL_0033:  brfalse.s  IL_004d

      IL_0035:  ldloc.2
      IL_0036:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_003b:  ldarg.0
      IL_003c:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0041:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0046:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_004b:  brfalse.s  IL_00b0

      IL_004d:  ldstr      "Id"
      IL_0052:  leave.s    IL_0054

      IL_0054:  ldloc.2
      IL_0055:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_005a:  dup
      IL_005b:  stloc.s    V_5
      IL_005d:  brfalse.s  IL_00a1

      IL_005f:  ldloc.s    V_5
      IL_0061:  call       string [mscorlib]System.String::IsInterned(string)
      IL_0066:  stloc.s    V_5
      IL_0068:  ldloc.s    V_5
      IL_006a:  ldstr      "Id"
      IL_006f:  bne.un.s   IL_00a1

      IL_0071:  ldarg.0
      IL_0072:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0077:  ldloc.0
      IL_0078:  ldloc.2
      IL_0079:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_007e:  stloc.1
      IL_007f:  ldarg.0
      IL_0080:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0085:  ldloc.0
      IL_0086:  ldstr      "HelpNamespace"
      IL_008b:  ldc.i4.1
      IL_008c:  newarr     [mscorlib]System.String
      IL_0091:  stloc.s    V_6
      IL_0093:  ldloc.s    V_6
      IL_0095:  ldc.i4.0
      IL_0096:  ldloc.1
      IL_0097:  stelem.ref
      IL_0098:  ldloc.s    V_6
      IL_009a:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateWixSimpleReferenceRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                            string,
                                                                                                                            string[])
      IL_009f:  br.s       IL_00bd

      IL_00a1:  ldarg.0
      IL_00a2:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00a7:  ldloc.0
      IL_00a8:  ldloc.2
      IL_00a9:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00ae:  br.s       IL_00bd

      IL_00b0:  ldarg.0
      IL_00b1:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00b6:  ldloc.0
      IL_00b7:  ldloc.2
      IL_00b8:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00bd:  ldloc.s    V_4
      IL_00bf:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00c4:  brtrue     IL_001b

      IL_00c9:  leave.s    IL_00e0

    }  // end .try
    finally
    {
      IL_00cb:  ldloc.s    V_4
      IL_00cd:  isinst     [mscorlib]System.IDisposable
      IL_00d2:  stloc.s    V_7
      IL_00d4:  ldloc.s    V_7
      IL_00d6:  brfalse.s  IL_00df

      IL_00d8:  ldloc.s    V_7
      IL_00da:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_00df:  endfinally
    }  // end handler
    IL_00e0:  ldloc.1
    IL_00e1:  brtrue.s   IL_00ff

    IL_00e3:  ldarg.0
    IL_00e4:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_00e9:  ldloc.0
    IL_00ea:  ldarg.1
    IL_00eb:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_00f0:  ldstr      "Id"
    IL_00f5:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_00fa:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_00ff:  ldarg.1
    IL_0100:  callvirt   instance class [System.Xml]System.Xml.XmlNodeList [System.Xml]System.Xml.XmlNode::get_ChildNodes()
    IL_0105:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNodeList::GetEnumerator()
    IL_010a:  stloc.s    V_4
    .try
    {
      IL_010c:  br.s       IL_0186

      IL_010e:  ldloc.s    V_4
      IL_0110:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0115:  castclass  [System.Xml]System.Xml.XmlNode
      IL_011a:  stloc.3
      IL_011b:  ldc.i4.1
      IL_011c:  ldloc.3
      IL_011d:  callvirt   instance valuetype [System.Xml]System.Xml.XmlNodeType [System.Xml]System.Xml.XmlNode::get_NodeType()
      IL_0122:  bne.un.s   IL_0186

      IL_0124:  ldloc.3
      IL_0125:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_012a:  ldarg.0
      IL_012b:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0130:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0135:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_013a:  brfalse.s  IL_0179

      IL_013c:  ldstr      "HelpFileRef"
      IL_0141:  leave.s    IL_0143

      IL_0143:  ldloc.3
      IL_0144:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_0149:  dup
      IL_014a:  stloc.s    V_5
      IL_014c:  brfalse.s  IL_016a

      IL_014e:  ldloc.s    V_5
      IL_0150:  call       string [mscorlib]System.String::IsInterned(string)
      IL_0155:  stloc.s    V_5
      IL_0157:  ldloc.s    V_5
      IL_0159:  ldstr      "HelpFileRef"
      IL_015e:  bne.un.s   IL_016a

      IL_0160:  ldarg.0
      IL_0161:  ldloc.3
      IL_0162:  ldloc.1
      IL_0163:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParseHelpFileRefElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            string)
      IL_0168:  br.s       IL_0186

      IL_016a:  ldarg.0
      IL_016b:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0170:  ldarg.1
      IL_0171:  ldloc.3
      IL_0172:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                  class [System.Xml]System.Xml.XmlNode)
      IL_0177:  br.s       IL_0186

      IL_0179:  ldarg.0
      IL_017a:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_017f:  ldarg.1
      IL_0180:  ldloc.3
      IL_0181:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            class [System.Xml]System.Xml.XmlNode)
      IL_0186:  ldloc.s    V_4
      IL_0188:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_018d:  brtrue     IL_010e

      IL_0192:  leave.s    IL_01a9

    }  // end .try
    finally
    {
      IL_0194:  ldloc.s    V_4
      IL_0196:  isinst     [mscorlib]System.IDisposable
      IL_019b:  stloc.s    V_7
      IL_019d:  ldloc.s    V_7
      IL_019f:  brfalse.s  IL_01a8

      IL_01a1:  ldloc.s    V_7
      IL_01a3:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_01a8:  endfinally
    }  // end handler
    IL_01a9:  ret
  } // end of method VSCompiler::ParseHelpCollectionRefElement

  .method private hidebysig instance void 
          ParseHelpCollectionElement(class [System.Xml]System.Xml.XmlNode node,
                                     string fileId) cil managed
  {
    // Code size       667 (0x29b)
    .maxstack  4
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection V_0,
             string V_1,
             string V_2,
             string V_3,
             class [System.Xml]System.Xml.XmlAttribute V_4,
             class [System.Xml]System.Xml.XmlNode V_5,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_6,
             class [mscorlib]System.Collections.IEnumerator V_7,
             string V_8,
             class [mscorlib]System.IDisposable V_9)
    IL_0000:  ldarg.1
    IL_0001:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Preprocessor::GetSourceLineNumbers(class [System.Xml]System.Xml.XmlNode)
    IL_0006:  stloc.0
    IL_0007:  ldnull
    IL_0008:  stloc.1
    IL_0009:  ldnull
    IL_000a:  stloc.2
    IL_000b:  ldnull
    IL_000c:  stloc.3
    IL_000d:  ldarg.1
    IL_000e:  callvirt   instance class [System.Xml]System.Xml.XmlAttributeCollection [System.Xml]System.Xml.XmlNode::get_Attributes()
    IL_0013:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNamedNodeMap::GetEnumerator()
    IL_0018:  stloc.s    V_7
    .try
    {
      IL_001a:  br         IL_00eb

      IL_001f:  ldloc.s    V_7
      IL_0021:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0026:  castclass  [System.Xml]System.Xml.XmlAttribute
      IL_002b:  stloc.s    V_4
      IL_002d:  ldloc.s    V_4
      IL_002f:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0034:  callvirt   instance int32 [mscorlib]System.String::get_Length()
      IL_0039:  brfalse.s  IL_0057

      IL_003b:  ldloc.s    V_4
      IL_003d:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0042:  ldarg.0
      IL_0043:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0048:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_004d:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_0052:  brfalse    IL_00dd

      IL_0057:  ldstr      "Id"
      IL_005c:  ldstr      "Description"
      IL_0061:  ldstr      "Name"
      IL_0066:  leave.s    IL_0068

      IL_0068:  ldloc.s    V_4
      IL_006a:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_006f:  dup
      IL_0070:  stloc.s    V_8
      IL_0072:  brfalse.s  IL_00cd

      IL_0074:  ldloc.s    V_8
      IL_0076:  call       string [mscorlib]System.String::IsInterned(string)
      IL_007b:  stloc.s    V_8
      IL_007d:  ldloc.s    V_8
      IL_007f:  ldstr      "Id"
      IL_0084:  beq.s      IL_009a

      IL_0086:  ldloc.s    V_8
      IL_0088:  ldstr      "Description"
      IL_008d:  beq.s      IL_00ab

      IL_008f:  ldloc.s    V_8
      IL_0091:  ldstr      "Name"
      IL_0096:  beq.s      IL_00bc

      IL_0098:  br.s       IL_00cd

      IL_009a:  ldarg.0
      IL_009b:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00a0:  ldloc.0
      IL_00a1:  ldloc.s    V_4
      IL_00a3:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00a8:  stloc.1
      IL_00a9:  br.s       IL_00eb

      IL_00ab:  ldarg.0
      IL_00ac:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00b1:  ldloc.0
      IL_00b2:  ldloc.s    V_4
      IL_00b4:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00b9:  stloc.2
      IL_00ba:  br.s       IL_00eb

      IL_00bc:  ldarg.0
      IL_00bd:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00c2:  ldloc.0
      IL_00c3:  ldloc.s    V_4
      IL_00c5:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00ca:  stloc.3
      IL_00cb:  br.s       IL_00eb

      IL_00cd:  ldarg.0
      IL_00ce:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00d3:  ldloc.0
      IL_00d4:  ldloc.s    V_4
      IL_00d6:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00db:  br.s       IL_00eb

      IL_00dd:  ldarg.0
      IL_00de:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00e3:  ldloc.0
      IL_00e4:  ldloc.s    V_4
      IL_00e6:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00eb:  ldloc.s    V_7
      IL_00ed:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00f2:  brtrue     IL_001f

      IL_00f7:  leave.s    IL_010e

    }  // end .try
    finally
    {
      IL_00f9:  ldloc.s    V_7
      IL_00fb:  isinst     [mscorlib]System.IDisposable
      IL_0100:  stloc.s    V_9
      IL_0102:  ldloc.s    V_9
      IL_0104:  brfalse.s  IL_010d

      IL_0106:  ldloc.s    V_9
      IL_0108:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_010d:  endfinally
    }  // end handler
    IL_010e:  ldloc.1
    IL_010f:  brtrue.s   IL_012d

    IL_0111:  ldarg.0
    IL_0112:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0117:  ldloc.0
    IL_0118:  ldarg.1
    IL_0119:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_011e:  ldstr      "Id"
    IL_0123:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_0128:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_012d:  ldloc.2
    IL_012e:  brtrue.s   IL_014c

    IL_0130:  ldarg.0
    IL_0131:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0136:  ldloc.0
    IL_0137:  ldarg.1
    IL_0138:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_013d:  ldstr      "Description"
    IL_0142:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_0147:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_014c:  ldloc.3
    IL_014d:  brtrue.s   IL_016b

    IL_014f:  ldarg.0
    IL_0150:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0155:  ldloc.0
    IL_0156:  ldarg.1
    IL_0157:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_015c:  ldstr      "Name"
    IL_0161:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_0166:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_016b:  ldarg.1
    IL_016c:  callvirt   instance class [System.Xml]System.Xml.XmlNodeList [System.Xml]System.Xml.XmlNode::get_ChildNodes()
    IL_0171:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNodeList::GetEnumerator()
    IL_0176:  stloc.s    V_7
    .try
    {
      IL_0178:  br         IL_0233

      IL_017d:  ldloc.s    V_7
      IL_017f:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0184:  castclass  [System.Xml]System.Xml.XmlNode
      IL_0189:  stloc.s    V_5
      IL_018b:  ldc.i4.1
      IL_018c:  ldloc.s    V_5
      IL_018e:  callvirt   instance valuetype [System.Xml]System.Xml.XmlNodeType [System.Xml]System.Xml.XmlNode::get_NodeType()
      IL_0193:  bne.un     IL_0233

      IL_0198:  ldloc.s    V_5
      IL_019a:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_019f:  ldarg.0
      IL_01a0:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_01a5:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_01aa:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_01af:  brfalse.s  IL_0225

      IL_01b1:  ldstr      "HelpFileRef"
      IL_01b6:  ldstr      "HelpFilterRef"
      IL_01bb:  ldstr      "PlugCollectionInto"
      IL_01c0:  leave.s    IL_01c2

      IL_01c2:  ldloc.s    V_5
      IL_01c4:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_01c9:  dup
      IL_01ca:  stloc.s    V_8
      IL_01cc:  brfalse.s  IL_0215

      IL_01ce:  ldloc.s    V_8
      IL_01d0:  call       string [mscorlib]System.String::IsInterned(string)
      IL_01d5:  stloc.s    V_8
      IL_01d7:  ldloc.s    V_8
      IL_01d9:  ldstr      "HelpFileRef"
      IL_01de:  beq.s      IL_01f4

      IL_01e0:  ldloc.s    V_8
      IL_01e2:  ldstr      "HelpFilterRef"
      IL_01e7:  beq.s      IL_01ff

      IL_01e9:  ldloc.s    V_8
      IL_01eb:  ldstr      "PlugCollectionInto"
      IL_01f0:  beq.s      IL_020a

      IL_01f2:  br.s       IL_0215

      IL_01f4:  ldarg.0
      IL_01f5:  ldloc.s    V_5
      IL_01f7:  ldloc.1
      IL_01f8:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParseHelpFileRefElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            string)
      IL_01fd:  br.s       IL_0233

      IL_01ff:  ldarg.0
      IL_0200:  ldloc.s    V_5
      IL_0202:  ldloc.1
      IL_0203:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParseHelpFilterRefElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                              string)
      IL_0208:  br.s       IL_0233

      IL_020a:  ldarg.0
      IL_020b:  ldloc.s    V_5
      IL_020d:  ldloc.1
      IL_020e:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::ParsePlugCollectionIntoElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                                   string)
      IL_0213:  br.s       IL_0233

      IL_0215:  ldarg.0
      IL_0216:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_021b:  ldarg.1
      IL_021c:  ldloc.s    V_5
      IL_021e:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                  class [System.Xml]System.Xml.XmlNode)
      IL_0223:  br.s       IL_0233

      IL_0225:  ldarg.0
      IL_0226:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_022b:  ldarg.1
      IL_022c:  ldloc.s    V_5
      IL_022e:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            class [System.Xml]System.Xml.XmlNode)
      IL_0233:  ldloc.s    V_7
      IL_0235:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_023a:  brtrue     IL_017d

      IL_023f:  leave.s    IL_0256

    }  // end .try
    finally
    {
      IL_0241:  ldloc.s    V_7
      IL_0243:  isinst     [mscorlib]System.IDisposable
      IL_0248:  stloc.s    V_9
      IL_024a:  ldloc.s    V_9
      IL_024c:  brfalse.s  IL_0255

      IL_024e:  ldloc.s    V_9
      IL_0250:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0255:  endfinally
    }  // end handler
    IL_0256:  ldarg.0
    IL_0257:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_025c:  callvirt   instance bool [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::get_EncounteredError()
    IL_0261:  brtrue.s   IL_029a

    IL_0263:  ldarg.0
    IL_0264:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0269:  ldloc.0
    IL_026a:  ldstr      "HelpNamespace"
    IL_026f:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Row [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                      string)
    IL_0274:  stloc.s    V_6
    IL_0276:  ldloc.s    V_6
    IL_0278:  ldc.i4.0
    IL_0279:  ldloc.1
    IL_027a:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_027f:  ldloc.s    V_6
    IL_0281:  ldc.i4.1
    IL_0282:  ldloc.3
    IL_0283:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0288:  ldloc.s    V_6
    IL_028a:  ldc.i4.2
    IL_028b:  ldarg.2
    IL_028c:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0291:  ldloc.s    V_6
    IL_0293:  ldc.i4.3
    IL_0294:  ldloc.2
    IL_0295:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_029a:  ret
  } // end of method VSCompiler::ParseHelpCollectionElement

  .method private hidebysig instance void 
          ParseHelpFileElement(class [System.Xml]System.Xml.XmlNode node,
                               string fileId) cil managed
  {
    // Code size       895 (0x37f)
    .maxstack  7
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection V_0,
             string V_1,
             string V_2,
             int32 V_3,
             string V_4,
             string V_5,
             string V_6,
             string V_7,
             class [System.Xml]System.Xml.XmlAttribute V_8,
             class [System.Xml]System.Xml.XmlNode V_9,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_10,
             class [mscorlib]System.Collections.IEnumerator V_11,
             string V_12,
             string[] V_13,
             class [mscorlib]System.IDisposable V_14)
    IL_0000:  ldarg.1
    IL_0001:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Preprocessor::GetSourceLineNumbers(class [System.Xml]System.Xml.XmlNode)
    IL_0006:  stloc.0
    IL_0007:  ldnull
    IL_0008:  stloc.1
    IL_0009:  ldnull
    IL_000a:  stloc.2
    IL_000b:  ldc.i4     0x80000000
    IL_0010:  stloc.3
    IL_0011:  ldnull
    IL_0012:  stloc.s    V_4
    IL_0014:  ldnull
    IL_0015:  stloc.s    V_5
    IL_0017:  ldnull
    IL_0018:  stloc.s    V_6
    IL_001a:  ldnull
    IL_001b:  stloc.s    V_7
    IL_001d:  ldarg.1
    IL_001e:  callvirt   instance class [System.Xml]System.Xml.XmlAttributeCollection [System.Xml]System.Xml.XmlNode::get_Attributes()
    IL_0023:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNamedNodeMap::GetEnumerator()
    IL_0028:  stloc.s    V_11
    .try
    {
      IL_002a:  br         IL_0226

      IL_002f:  ldloc.s    V_11
      IL_0031:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0036:  castclass  [System.Xml]System.Xml.XmlAttribute
      IL_003b:  stloc.s    V_8
      IL_003d:  ldloc.s    V_8
      IL_003f:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0044:  callvirt   instance int32 [mscorlib]System.String::get_Length()
      IL_0049:  brfalse.s  IL_0067

      IL_004b:  ldloc.s    V_8
      IL_004d:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0052:  ldarg.0
      IL_0053:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0058:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_005d:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_0062:  brfalse    IL_0218

      IL_0067:  ldstr      "Id"
      IL_006c:  ldstr      "AttributeIndex"
      IL_0071:  ldstr      "Index"
      IL_0076:  ldstr      "Language"
      IL_007b:  ldstr      "Name"
      IL_0080:  ldstr      "SampleLocation"
      IL_0085:  ldstr      "Search"
      IL_008a:  leave.s    IL_008c

      IL_008c:  ldloc.s    V_8
      IL_008e:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_0093:  dup
      IL_0094:  stloc.s    V_12
      IL_0096:  brfalse    IL_0208

      IL_009b:  ldloc.s    V_12
      IL_009d:  call       string [mscorlib]System.String::IsInterned(string)
      IL_00a2:  stloc.s    V_12
      IL_00a4:  ldloc.s    V_12
      IL_00a6:  ldstr      "Id"
      IL_00ab:  beq.s      IL_00f4

      IL_00ad:  ldloc.s    V_12
      IL_00af:  ldstr      "AttributeIndex"
      IL_00b4:  beq.s      IL_0108

      IL_00b6:  ldloc.s    V_12
      IL_00b8:  ldstr      "Index"
      IL_00bd:  beq.s      IL_013e

      IL_00bf:  ldloc.s    V_12
      IL_00c1:  ldstr      "Language"
      IL_00c6:  beq        IL_0174

      IL_00cb:  ldloc.s    V_12
      IL_00cd:  ldstr      "Name"
      IL_00d2:  beq        IL_018e

      IL_00d7:  ldloc.s    V_12
      IL_00d9:  ldstr      "SampleLocation"
      IL_00de:  beq        IL_01a2

      IL_00e3:  ldloc.s    V_12
      IL_00e5:  ldstr      "Search"
      IL_00ea:  beq        IL_01d5

      IL_00ef:  br         IL_0208

      IL_00f4:  ldarg.0
      IL_00f5:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00fa:  ldloc.0
      IL_00fb:  ldloc.s    V_8
      IL_00fd:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_0102:  stloc.1
      IL_0103:  br         IL_0226

      IL_0108:  ldarg.0
      IL_0109:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_010e:  ldloc.0
      IL_010f:  ldloc.s    V_8
      IL_0111:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_0116:  stloc.s    V_6
      IL_0118:  ldarg.0
      IL_0119:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_011e:  ldloc.0
      IL_011f:  ldstr      "File"
      IL_0124:  ldc.i4.1
      IL_0125:  newarr     [mscorlib]System.String
      IL_012a:  stloc.s    V_13
      IL_012c:  ldloc.s    V_13
      IL_012e:  ldc.i4.0
      IL_012f:  ldloc.s    V_6
      IL_0131:  stelem.ref
      IL_0132:  ldloc.s    V_13
      IL_0134:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateWixSimpleReferenceRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                            string,
                                                                                                                            string[])
      IL_0139:  br         IL_0226

      IL_013e:  ldarg.0
      IL_013f:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0144:  ldloc.0
      IL_0145:  ldloc.s    V_8
      IL_0147:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_014c:  stloc.s    V_4
      IL_014e:  ldarg.0
      IL_014f:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0154:  ldloc.0
      IL_0155:  ldstr      "File"
      IL_015a:  ldc.i4.1
      IL_015b:  newarr     [mscorlib]System.String
      IL_0160:  stloc.s    V_13
      IL_0162:  ldloc.s    V_13
      IL_0164:  ldc.i4.0
      IL_0165:  ldloc.s    V_4
      IL_0167:  stelem.ref
      IL_0168:  ldloc.s    V_13
      IL_016a:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateWixSimpleReferenceRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                            string,
                                                                                                                            string[])
      IL_016f:  br         IL_0226

      IL_0174:  ldarg.0
      IL_0175:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_017a:  ldloc.0
      IL_017b:  ldloc.s    V_8
      IL_017d:  ldc.i4.0
      IL_017e:  ldc.i4     0x7fff
      IL_0183:  callvirt   instance int32 [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIntegerValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                          class [System.Xml]System.Xml.XmlAttribute,
                                                                                                                          int32,
                                                                                                                          int32)
      IL_0188:  stloc.3
      IL_0189:  br         IL_0226

      IL_018e:  ldarg.0
      IL_018f:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0194:  ldloc.0
      IL_0195:  ldloc.s    V_8
      IL_0197:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_019c:  stloc.2
      IL_019d:  br         IL_0226

      IL_01a2:  ldarg.0
      IL_01a3:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_01a8:  ldloc.0
      IL_01a9:  ldloc.s    V_8
      IL_01ab:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_01b0:  stloc.s    V_7
      IL_01b2:  ldarg.0
      IL_01b3:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_01b8:  ldloc.0
      IL_01b9:  ldstr      "File"
      IL_01be:  ldc.i4.1
      IL_01bf:  newarr     [mscorlib]System.String
      IL_01c4:  stloc.s    V_13
      IL_01c6:  ldloc.s    V_13
      IL_01c8:  ldc.i4.0
      IL_01c9:  ldloc.s    V_7
      IL_01cb:  stelem.ref
      IL_01cc:  ldloc.s    V_13
      IL_01ce:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateWixSimpleReferenceRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                            string,
                                                                                                                            string[])
      IL_01d3:  br.s       IL_0226

      IL_01d5:  ldarg.0
      IL_01d6:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_01db:  ldloc.0
      IL_01dc:  ldloc.s    V_8
      IL_01de:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_01e3:  stloc.s    V_5
      IL_01e5:  ldarg.0
      IL_01e6:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_01eb:  ldloc.0
      IL_01ec:  ldstr      "File"
      IL_01f1:  ldc.i4.1
      IL_01f2:  newarr     [mscorlib]System.String
      IL_01f7:  stloc.s    V_13
      IL_01f9:  ldloc.s    V_13
      IL_01fb:  ldc.i4.0
      IL_01fc:  ldloc.s    V_5
      IL_01fe:  stelem.ref
      IL_01ff:  ldloc.s    V_13
      IL_0201:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateWixSimpleReferenceRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                            string,
                                                                                                                            string[])
      IL_0206:  br.s       IL_0226

      IL_0208:  ldarg.0
      IL_0209:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_020e:  ldloc.0
      IL_020f:  ldloc.s    V_8
      IL_0211:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_0216:  br.s       IL_0226

      IL_0218:  ldarg.0
      IL_0219:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_021e:  ldloc.0
      IL_021f:  ldloc.s    V_8
      IL_0221:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_0226:  ldloc.s    V_11
      IL_0228:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_022d:  brtrue     IL_002f

      IL_0232:  leave.s    IL_0249

    }  // end .try
    finally
    {
      IL_0234:  ldloc.s    V_11
      IL_0236:  isinst     [mscorlib]System.IDisposable
      IL_023b:  stloc.s    V_14
      IL_023d:  ldloc.s    V_14
      IL_023f:  brfalse.s  IL_0248

      IL_0241:  ldloc.s    V_14
      IL_0243:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0248:  endfinally
    }  // end handler
    IL_0249:  ldloc.1
    IL_024a:  brtrue.s   IL_0268

    IL_024c:  ldarg.0
    IL_024d:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0252:  ldloc.0
    IL_0253:  ldarg.1
    IL_0254:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_0259:  ldstr      "Id"
    IL_025e:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_0263:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_0268:  ldloc.2
    IL_0269:  brtrue.s   IL_0287

    IL_026b:  ldarg.0
    IL_026c:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0271:  ldloc.0
    IL_0272:  ldarg.1
    IL_0273:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_0278:  ldstr      "Name"
    IL_027d:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_0282:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_0287:  ldarg.1
    IL_0288:  callvirt   instance class [System.Xml]System.Xml.XmlNodeList [System.Xml]System.Xml.XmlNode::get_ChildNodes()
    IL_028d:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNodeList::GetEnumerator()
    IL_0292:  stloc.s    V_11
    .try
    {
      IL_0294:  br.s       IL_02e5

      IL_0296:  ldloc.s    V_11
      IL_0298:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_029d:  castclass  [System.Xml]System.Xml.XmlNode
      IL_02a2:  stloc.s    V_9
      IL_02a4:  ldc.i4.1
      IL_02a5:  ldloc.s    V_9
      IL_02a7:  callvirt   instance valuetype [System.Xml]System.Xml.XmlNodeType [System.Xml]System.Xml.XmlNode::get_NodeType()
      IL_02ac:  bne.un.s   IL_02e5

      IL_02ae:  ldloc.s    V_9
      IL_02b0:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_02b5:  ldarg.0
      IL_02b6:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_02bb:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_02c0:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_02c5:  brfalse.s  IL_02d7

      IL_02c7:  ldarg.0
      IL_02c8:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_02cd:  ldarg.1
      IL_02ce:  ldloc.s    V_9
      IL_02d0:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                  class [System.Xml]System.Xml.XmlNode)
      IL_02d5:  br.s       IL_02e5

      IL_02d7:  ldarg.0
      IL_02d8:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_02dd:  ldarg.1
      IL_02de:  ldloc.s    V_9
      IL_02e0:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            class [System.Xml]System.Xml.XmlNode)
      IL_02e5:  ldloc.s    V_11
      IL_02e7:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_02ec:  brtrue.s   IL_0296

      IL_02ee:  leave.s    IL_0305

    }  // end .try
    finally
    {
      IL_02f0:  ldloc.s    V_11
      IL_02f2:  isinst     [mscorlib]System.IDisposable
      IL_02f7:  stloc.s    V_14
      IL_02f9:  ldloc.s    V_14
      IL_02fb:  brfalse.s  IL_0304

      IL_02fd:  ldloc.s    V_14
      IL_02ff:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0304:  endfinally
    }  // end handler
    IL_0305:  ldarg.0
    IL_0306:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_030b:  callvirt   instance bool [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::get_EncounteredError()
    IL_0310:  brtrue.s   IL_037e

    IL_0312:  ldarg.0
    IL_0313:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0318:  ldloc.0
    IL_0319:  ldstr      "HelpFile"
    IL_031e:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Row [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                      string)
    IL_0323:  stloc.s    V_10
    IL_0325:  ldloc.s    V_10
    IL_0327:  ldc.i4.0
    IL_0328:  ldloc.1
    IL_0329:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_032e:  ldloc.s    V_10
    IL_0330:  ldc.i4.1
    IL_0331:  ldloc.2
    IL_0332:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0337:  ldc.i4     0x80000000
    IL_033c:  ldloc.3
    IL_033d:  beq.s      IL_034d

    IL_033f:  ldloc.s    V_10
    IL_0341:  ldc.i4.2
    IL_0342:  ldloc.3
    IL_0343:  box        [mscorlib]System.Int32
    IL_0348:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_034d:  ldloc.s    V_10
    IL_034f:  ldc.i4.3
    IL_0350:  ldarg.2
    IL_0351:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0356:  ldloc.s    V_10
    IL_0358:  ldc.i4.4
    IL_0359:  ldloc.s    V_4
    IL_035b:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0360:  ldloc.s    V_10
    IL_0362:  ldc.i4.5
    IL_0363:  ldloc.s    V_5
    IL_0365:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_036a:  ldloc.s    V_10
    IL_036c:  ldc.i4.6
    IL_036d:  ldloc.s    V_6
    IL_036f:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0374:  ldloc.s    V_10
    IL_0376:  ldc.i4.7
    IL_0377:  ldloc.s    V_7
    IL_0379:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_037e:  ret
  } // end of method VSCompiler::ParseHelpFileElement

  .method private hidebysig instance void 
          ParseHelpFileRefElement(class [System.Xml]System.Xml.XmlNode node,
                                  string collectionId) cil managed
  {
    // Code size       427 (0x1ab)
    .maxstack  6
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection V_0,
             string V_1,
             class [System.Xml]System.Xml.XmlAttribute V_2,
             class [System.Xml]System.Xml.XmlNode V_3,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_4,
             class [mscorlib]System.Collections.IEnumerator V_5,
             string V_6,
             string[] V_7,
             class [mscorlib]System.IDisposable V_8)
    IL_0000:  ldarg.1
    IL_0001:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Preprocessor::GetSourceLineNumbers(class [System.Xml]System.Xml.XmlNode)
    IL_0006:  stloc.0
    IL_0007:  ldnull
    IL_0008:  stloc.1
    IL_0009:  ldarg.1
    IL_000a:  callvirt   instance class [System.Xml]System.Xml.XmlAttributeCollection [System.Xml]System.Xml.XmlNode::get_Attributes()
    IL_000f:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNamedNodeMap::GetEnumerator()
    IL_0014:  stloc.s    V_5
    .try
    {
      IL_0016:  br         IL_00bd

      IL_001b:  ldloc.s    V_5
      IL_001d:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0022:  castclass  [System.Xml]System.Xml.XmlAttribute
      IL_0027:  stloc.2
      IL_0028:  ldloc.2
      IL_0029:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_002e:  callvirt   instance int32 [mscorlib]System.String::get_Length()
      IL_0033:  brfalse.s  IL_004d

      IL_0035:  ldloc.2
      IL_0036:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_003b:  ldarg.0
      IL_003c:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0041:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0046:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_004b:  brfalse.s  IL_00b0

      IL_004d:  ldstr      "Id"
      IL_0052:  leave.s    IL_0054

      IL_0054:  ldloc.2
      IL_0055:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_005a:  dup
      IL_005b:  stloc.s    V_6
      IL_005d:  brfalse.s  IL_00a1

      IL_005f:  ldloc.s    V_6
      IL_0061:  call       string [mscorlib]System.String::IsInterned(string)
      IL_0066:  stloc.s    V_6
      IL_0068:  ldloc.s    V_6
      IL_006a:  ldstr      "Id"
      IL_006f:  bne.un.s   IL_00a1

      IL_0071:  ldarg.0
      IL_0072:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0077:  ldloc.0
      IL_0078:  ldloc.2
      IL_0079:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_007e:  stloc.1
      IL_007f:  ldarg.0
      IL_0080:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0085:  ldloc.0
      IL_0086:  ldstr      "HelpFile"
      IL_008b:  ldc.i4.1
      IL_008c:  newarr     [mscorlib]System.String
      IL_0091:  stloc.s    V_7
      IL_0093:  ldloc.s    V_7
      IL_0095:  ldc.i4.0
      IL_0096:  ldloc.1
      IL_0097:  stelem.ref
      IL_0098:  ldloc.s    V_7
      IL_009a:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateWixSimpleReferenceRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                            string,
                                                                                                                            string[])
      IL_009f:  br.s       IL_00bd

      IL_00a1:  ldarg.0
      IL_00a2:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00a7:  ldloc.0
      IL_00a8:  ldloc.2
      IL_00a9:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00ae:  br.s       IL_00bd

      IL_00b0:  ldarg.0
      IL_00b1:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00b6:  ldloc.0
      IL_00b7:  ldloc.2
      IL_00b8:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00bd:  ldloc.s    V_5
      IL_00bf:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00c4:  brtrue     IL_001b

      IL_00c9:  leave.s    IL_00e0

    }  // end .try
    finally
    {
      IL_00cb:  ldloc.s    V_5
      IL_00cd:  isinst     [mscorlib]System.IDisposable
      IL_00d2:  stloc.s    V_8
      IL_00d4:  ldloc.s    V_8
      IL_00d6:  brfalse.s  IL_00df

      IL_00d8:  ldloc.s    V_8
      IL_00da:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_00df:  endfinally
    }  // end handler
    IL_00e0:  ldloc.1
    IL_00e1:  brtrue.s   IL_00ff

    IL_00e3:  ldarg.0
    IL_00e4:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_00e9:  ldloc.0
    IL_00ea:  ldarg.1
    IL_00eb:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_00f0:  ldstr      "Id"
    IL_00f5:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_00fa:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_00ff:  ldarg.1
    IL_0100:  callvirt   instance class [System.Xml]System.Xml.XmlNodeList [System.Xml]System.Xml.XmlNode::get_ChildNodes()
    IL_0105:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNodeList::GetEnumerator()
    IL_010a:  stloc.s    V_5
    .try
    {
      IL_010c:  br.s       IL_0158

      IL_010e:  ldloc.s    V_5
      IL_0110:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0115:  castclass  [System.Xml]System.Xml.XmlNode
      IL_011a:  stloc.3
      IL_011b:  ldc.i4.1
      IL_011c:  ldloc.3
      IL_011d:  callvirt   instance valuetype [System.Xml]System.Xml.XmlNodeType [System.Xml]System.Xml.XmlNode::get_NodeType()
      IL_0122:  bne.un.s   IL_0158

      IL_0124:  ldloc.3
      IL_0125:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_012a:  ldarg.0
      IL_012b:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0130:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0135:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_013a:  brfalse.s  IL_014b

      IL_013c:  ldarg.0
      IL_013d:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0142:  ldarg.1
      IL_0143:  ldloc.3
      IL_0144:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                  class [System.Xml]System.Xml.XmlNode)
      IL_0149:  br.s       IL_0158

      IL_014b:  ldarg.0
      IL_014c:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0151:  ldarg.1
      IL_0152:  ldloc.3
      IL_0153:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            class [System.Xml]System.Xml.XmlNode)
      IL_0158:  ldloc.s    V_5
      IL_015a:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_015f:  brtrue.s   IL_010e

      IL_0161:  leave.s    IL_0178

    }  // end .try
    finally
    {
      IL_0163:  ldloc.s    V_5
      IL_0165:  isinst     [mscorlib]System.IDisposable
      IL_016a:  stloc.s    V_8
      IL_016c:  ldloc.s    V_8
      IL_016e:  brfalse.s  IL_0177

      IL_0170:  ldloc.s    V_8
      IL_0172:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0177:  endfinally
    }  // end handler
    IL_0178:  ldarg.0
    IL_0179:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_017e:  callvirt   instance bool [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::get_EncounteredError()
    IL_0183:  brtrue.s   IL_01aa

    IL_0185:  ldarg.0
    IL_0186:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_018b:  ldloc.0
    IL_018c:  ldstr      "HelpFileToNamespace"
    IL_0191:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Row [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                      string)
    IL_0196:  stloc.s    V_4
    IL_0198:  ldloc.s    V_4
    IL_019a:  ldc.i4.0
    IL_019b:  ldloc.1
    IL_019c:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_01a1:  ldloc.s    V_4
    IL_01a3:  ldc.i4.1
    IL_01a4:  ldarg.2
    IL_01a5:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_01aa:  ret
  } // end of method VSCompiler::ParseHelpFileRefElement

  .method private hidebysig instance void 
          ParseHelpFilterElement(class [System.Xml]System.Xml.XmlNode node) cil managed
  {
    // Code size       518 (0x206)
    .maxstack  4
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection V_0,
             string V_1,
             string V_2,
             string V_3,
             class [System.Xml]System.Xml.XmlAttribute V_4,
             class [System.Xml]System.Xml.XmlNode V_5,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_6,
             class [mscorlib]System.Collections.IEnumerator V_7,
             string V_8,
             class [mscorlib]System.IDisposable V_9)
    IL_0000:  ldarg.1
    IL_0001:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Preprocessor::GetSourceLineNumbers(class [System.Xml]System.Xml.XmlNode)
    IL_0006:  stloc.0
    IL_0007:  ldnull
    IL_0008:  stloc.1
    IL_0009:  ldnull
    IL_000a:  stloc.2
    IL_000b:  ldnull
    IL_000c:  stloc.3
    IL_000d:  ldarg.1
    IL_000e:  callvirt   instance class [System.Xml]System.Xml.XmlAttributeCollection [System.Xml]System.Xml.XmlNode::get_Attributes()
    IL_0013:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNamedNodeMap::GetEnumerator()
    IL_0018:  stloc.s    V_7
    .try
    {
      IL_001a:  br         IL_00eb

      IL_001f:  ldloc.s    V_7
      IL_0021:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0026:  castclass  [System.Xml]System.Xml.XmlAttribute
      IL_002b:  stloc.s    V_4
      IL_002d:  ldloc.s    V_4
      IL_002f:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0034:  callvirt   instance int32 [mscorlib]System.String::get_Length()
      IL_0039:  brfalse.s  IL_0057

      IL_003b:  ldloc.s    V_4
      IL_003d:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0042:  ldarg.0
      IL_0043:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0048:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_004d:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_0052:  brfalse    IL_00dd

      IL_0057:  ldstr      "Id"
      IL_005c:  ldstr      "FilterDefinition"
      IL_0061:  ldstr      "Name"
      IL_0066:  leave.s    IL_0068

      IL_0068:  ldloc.s    V_4
      IL_006a:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_006f:  dup
      IL_0070:  stloc.s    V_8
      IL_0072:  brfalse.s  IL_00cd

      IL_0074:  ldloc.s    V_8
      IL_0076:  call       string [mscorlib]System.String::IsInterned(string)
      IL_007b:  stloc.s    V_8
      IL_007d:  ldloc.s    V_8
      IL_007f:  ldstr      "Id"
      IL_0084:  beq.s      IL_009a

      IL_0086:  ldloc.s    V_8
      IL_0088:  ldstr      "FilterDefinition"
      IL_008d:  beq.s      IL_00ab

      IL_008f:  ldloc.s    V_8
      IL_0091:  ldstr      "Name"
      IL_0096:  beq.s      IL_00bc

      IL_0098:  br.s       IL_00cd

      IL_009a:  ldarg.0
      IL_009b:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00a0:  ldloc.0
      IL_00a1:  ldloc.s    V_4
      IL_00a3:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00a8:  stloc.1
      IL_00a9:  br.s       IL_00eb

      IL_00ab:  ldarg.0
      IL_00ac:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00b1:  ldloc.0
      IL_00b2:  ldloc.s    V_4
      IL_00b4:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00b9:  stloc.2
      IL_00ba:  br.s       IL_00eb

      IL_00bc:  ldarg.0
      IL_00bd:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00c2:  ldloc.0
      IL_00c3:  ldloc.s    V_4
      IL_00c5:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00ca:  stloc.3
      IL_00cb:  br.s       IL_00eb

      IL_00cd:  ldarg.0
      IL_00ce:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00d3:  ldloc.0
      IL_00d4:  ldloc.s    V_4
      IL_00d6:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00db:  br.s       IL_00eb

      IL_00dd:  ldarg.0
      IL_00de:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00e3:  ldloc.0
      IL_00e4:  ldloc.s    V_4
      IL_00e6:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00eb:  ldloc.s    V_7
      IL_00ed:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00f2:  brtrue     IL_001f

      IL_00f7:  leave.s    IL_010e

    }  // end .try
    finally
    {
      IL_00f9:  ldloc.s    V_7
      IL_00fb:  isinst     [mscorlib]System.IDisposable
      IL_0100:  stloc.s    V_9
      IL_0102:  ldloc.s    V_9
      IL_0104:  brfalse.s  IL_010d

      IL_0106:  ldloc.s    V_9
      IL_0108:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_010d:  endfinally
    }  // end handler
    IL_010e:  ldloc.1
    IL_010f:  brtrue.s   IL_012d

    IL_0111:  ldarg.0
    IL_0112:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0117:  ldloc.0
    IL_0118:  ldarg.1
    IL_0119:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_011e:  ldstr      "Id"
    IL_0123:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_0128:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_012d:  ldloc.3
    IL_012e:  brtrue.s   IL_014c

    IL_0130:  ldarg.0
    IL_0131:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_0136:  ldloc.0
    IL_0137:  ldarg.1
    IL_0138:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_013d:  ldstr      "Name"
    IL_0142:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_0147:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_014c:  ldarg.1
    IL_014d:  callvirt   instance class [System.Xml]System.Xml.XmlNodeList [System.Xml]System.Xml.XmlNode::get_ChildNodes()
    IL_0152:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNodeList::GetEnumerator()
    IL_0157:  stloc.s    V_7
    .try
    {
      IL_0159:  br.s       IL_01aa

      IL_015b:  ldloc.s    V_7
      IL_015d:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0162:  castclass  [System.Xml]System.Xml.XmlNode
      IL_0167:  stloc.s    V_5
      IL_0169:  ldc.i4.1
      IL_016a:  ldloc.s    V_5
      IL_016c:  callvirt   instance valuetype [System.Xml]System.Xml.XmlNodeType [System.Xml]System.Xml.XmlNode::get_NodeType()
      IL_0171:  bne.un.s   IL_01aa

      IL_0173:  ldloc.s    V_5
      IL_0175:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_017a:  ldarg.0
      IL_017b:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0180:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0185:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_018a:  brfalse.s  IL_019c

      IL_018c:  ldarg.0
      IL_018d:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0192:  ldarg.1
      IL_0193:  ldloc.s    V_5
      IL_0195:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                  class [System.Xml]System.Xml.XmlNode)
      IL_019a:  br.s       IL_01aa

      IL_019c:  ldarg.0
      IL_019d:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_01a2:  ldarg.1
      IL_01a3:  ldloc.s    V_5
      IL_01a5:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            class [System.Xml]System.Xml.XmlNode)
      IL_01aa:  ldloc.s    V_7
      IL_01ac:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_01b1:  brtrue.s   IL_015b

      IL_01b3:  leave.s    IL_01ca

    }  // end .try
    finally
    {
      IL_01b5:  ldloc.s    V_7
      IL_01b7:  isinst     [mscorlib]System.IDisposable
      IL_01bc:  stloc.s    V_9
      IL_01be:  ldloc.s    V_9
      IL_01c0:  brfalse.s  IL_01c9

      IL_01c2:  ldloc.s    V_9
      IL_01c4:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_01c9:  endfinally
    }  // end handler
    IL_01ca:  ldarg.0
    IL_01cb:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_01d0:  callvirt   instance bool [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::get_EncounteredError()
    IL_01d5:  brtrue.s   IL_0205

    IL_01d7:  ldarg.0
    IL_01d8:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_01dd:  ldloc.0
    IL_01de:  ldstr      "HelpFilter"
    IL_01e3:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Row [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                      string)
    IL_01e8:  stloc.s    V_6
    IL_01ea:  ldloc.s    V_6
    IL_01ec:  ldc.i4.0
    IL_01ed:  ldloc.1
    IL_01ee:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_01f3:  ldloc.s    V_6
    IL_01f5:  ldc.i4.1
    IL_01f6:  ldloc.3
    IL_01f7:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_01fc:  ldloc.s    V_6
    IL_01fe:  ldc.i4.2
    IL_01ff:  ldloc.2
    IL_0200:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0205:  ret
  } // end of method VSCompiler::ParseHelpFilterElement

  .method private hidebysig instance void 
          ParseHelpFilterRefElement(class [System.Xml]System.Xml.XmlNode node,
                                    string collectionId) cil managed
  {
    // Code size       427 (0x1ab)
    .maxstack  6
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection V_0,
             string V_1,
             class [System.Xml]System.Xml.XmlAttribute V_2,
             class [System.Xml]System.Xml.XmlNode V_3,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_4,
             class [mscorlib]System.Collections.IEnumerator V_5,
             string V_6,
             string[] V_7,
             class [mscorlib]System.IDisposable V_8)
    IL_0000:  ldarg.1
    IL_0001:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Preprocessor::GetSourceLineNumbers(class [System.Xml]System.Xml.XmlNode)
    IL_0006:  stloc.0
    IL_0007:  ldnull
    IL_0008:  stloc.1
    IL_0009:  ldarg.1
    IL_000a:  callvirt   instance class [System.Xml]System.Xml.XmlAttributeCollection [System.Xml]System.Xml.XmlNode::get_Attributes()
    IL_000f:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNamedNodeMap::GetEnumerator()
    IL_0014:  stloc.s    V_5
    .try
    {
      IL_0016:  br         IL_00bd

      IL_001b:  ldloc.s    V_5
      IL_001d:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0022:  castclass  [System.Xml]System.Xml.XmlAttribute
      IL_0027:  stloc.2
      IL_0028:  ldloc.2
      IL_0029:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_002e:  callvirt   instance int32 [mscorlib]System.String::get_Length()
      IL_0033:  brfalse.s  IL_004d

      IL_0035:  ldloc.2
      IL_0036:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_003b:  ldarg.0
      IL_003c:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0041:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0046:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_004b:  brfalse.s  IL_00b0

      IL_004d:  ldstr      "Id"
      IL_0052:  leave.s    IL_0054

      IL_0054:  ldloc.2
      IL_0055:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_005a:  dup
      IL_005b:  stloc.s    V_6
      IL_005d:  brfalse.s  IL_00a1

      IL_005f:  ldloc.s    V_6
      IL_0061:  call       string [mscorlib]System.String::IsInterned(string)
      IL_0066:  stloc.s    V_6
      IL_0068:  ldloc.s    V_6
      IL_006a:  ldstr      "Id"
      IL_006f:  bne.un.s   IL_00a1

      IL_0071:  ldarg.0
      IL_0072:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0077:  ldloc.0
      IL_0078:  ldloc.2
      IL_0079:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_007e:  stloc.1
      IL_007f:  ldarg.0
      IL_0080:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0085:  ldloc.0
      IL_0086:  ldstr      "HelpFilter"
      IL_008b:  ldc.i4.1
      IL_008c:  newarr     [mscorlib]System.String
      IL_0091:  stloc.s    V_7
      IL_0093:  ldloc.s    V_7
      IL_0095:  ldc.i4.0
      IL_0096:  ldloc.1
      IL_0097:  stelem.ref
      IL_0098:  ldloc.s    V_7
      IL_009a:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateWixSimpleReferenceRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                            string,
                                                                                                                            string[])
      IL_009f:  br.s       IL_00bd

      IL_00a1:  ldarg.0
      IL_00a2:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00a7:  ldloc.0
      IL_00a8:  ldloc.2
      IL_00a9:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00ae:  br.s       IL_00bd

      IL_00b0:  ldarg.0
      IL_00b1:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00b6:  ldloc.0
      IL_00b7:  ldloc.2
      IL_00b8:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00bd:  ldloc.s    V_5
      IL_00bf:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00c4:  brtrue     IL_001b

      IL_00c9:  leave.s    IL_00e0

    }  // end .try
    finally
    {
      IL_00cb:  ldloc.s    V_5
      IL_00cd:  isinst     [mscorlib]System.IDisposable
      IL_00d2:  stloc.s    V_8
      IL_00d4:  ldloc.s    V_8
      IL_00d6:  brfalse.s  IL_00df

      IL_00d8:  ldloc.s    V_8
      IL_00da:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_00df:  endfinally
    }  // end handler
    IL_00e0:  ldloc.1
    IL_00e1:  brtrue.s   IL_00ff

    IL_00e3:  ldarg.0
    IL_00e4:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_00e9:  ldloc.0
    IL_00ea:  ldarg.1
    IL_00eb:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_00f0:  ldstr      "Id"
    IL_00f5:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_00fa:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_00ff:  ldarg.1
    IL_0100:  callvirt   instance class [System.Xml]System.Xml.XmlNodeList [System.Xml]System.Xml.XmlNode::get_ChildNodes()
    IL_0105:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNodeList::GetEnumerator()
    IL_010a:  stloc.s    V_5
    .try
    {
      IL_010c:  br.s       IL_0158

      IL_010e:  ldloc.s    V_5
      IL_0110:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0115:  castclass  [System.Xml]System.Xml.XmlNode
      IL_011a:  stloc.3
      IL_011b:  ldc.i4.1
      IL_011c:  ldloc.3
      IL_011d:  callvirt   instance valuetype [System.Xml]System.Xml.XmlNodeType [System.Xml]System.Xml.XmlNode::get_NodeType()
      IL_0122:  bne.un.s   IL_0158

      IL_0124:  ldloc.3
      IL_0125:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_012a:  ldarg.0
      IL_012b:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0130:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0135:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_013a:  brfalse.s  IL_014b

      IL_013c:  ldarg.0
      IL_013d:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0142:  ldarg.1
      IL_0143:  ldloc.3
      IL_0144:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                  class [System.Xml]System.Xml.XmlNode)
      IL_0149:  br.s       IL_0158

      IL_014b:  ldarg.0
      IL_014c:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0151:  ldarg.1
      IL_0152:  ldloc.3
      IL_0153:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            class [System.Xml]System.Xml.XmlNode)
      IL_0158:  ldloc.s    V_5
      IL_015a:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_015f:  brtrue.s   IL_010e

      IL_0161:  leave.s    IL_0178

    }  // end .try
    finally
    {
      IL_0163:  ldloc.s    V_5
      IL_0165:  isinst     [mscorlib]System.IDisposable
      IL_016a:  stloc.s    V_8
      IL_016c:  ldloc.s    V_8
      IL_016e:  brfalse.s  IL_0177

      IL_0170:  ldloc.s    V_8
      IL_0172:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0177:  endfinally
    }  // end handler
    IL_0178:  ldarg.0
    IL_0179:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_017e:  callvirt   instance bool [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::get_EncounteredError()
    IL_0183:  brtrue.s   IL_01aa

    IL_0185:  ldarg.0
    IL_0186:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_018b:  ldloc.0
    IL_018c:  ldstr      "HelpFilterToNamespace"
    IL_0191:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Row [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                      string)
    IL_0196:  stloc.s    V_4
    IL_0198:  ldloc.s    V_4
    IL_019a:  ldc.i4.0
    IL_019b:  ldloc.1
    IL_019c:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_01a1:  ldloc.s    V_4
    IL_01a3:  ldc.i4.1
    IL_01a4:  ldarg.2
    IL_01a5:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_01aa:  ret
  } // end of method VSCompiler::ParseHelpFilterRefElement

  .method private hidebysig instance void 
          ParsePlugCollectionIntoElement(class [System.Xml]System.Xml.XmlNode node,
                                         string parentId) cil managed
  {
    // Code size       542 (0x21e)
    .maxstack  4
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection V_0,
             string V_1,
             string V_2,
             string V_3,
             string V_4,
             class [System.Xml]System.Xml.XmlAttribute V_5,
             class [System.Xml]System.Xml.XmlNode V_6,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_7,
             class [mscorlib]System.Collections.IEnumerator V_8,
             string V_9,
             class [mscorlib]System.IDisposable V_10)
    IL_0000:  ldarg.1
    IL_0001:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Preprocessor::GetSourceLineNumbers(class [System.Xml]System.Xml.XmlNode)
    IL_0006:  stloc.0
    IL_0007:  ldnull
    IL_0008:  stloc.1
    IL_0009:  ldnull
    IL_000a:  stloc.2
    IL_000b:  ldnull
    IL_000c:  stloc.3
    IL_000d:  ldnull
    IL_000e:  stloc.s    V_4
    IL_0010:  ldarg.1
    IL_0011:  callvirt   instance class [System.Xml]System.Xml.XmlAttributeCollection [System.Xml]System.Xml.XmlNode::get_Attributes()
    IL_0016:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNamedNodeMap::GetEnumerator()
    IL_001b:  stloc.s    V_8
    .try
    {
      IL_001d:  br         IL_010e

      IL_0022:  ldloc.s    V_8
      IL_0024:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0029:  castclass  [System.Xml]System.Xml.XmlAttribute
      IL_002e:  stloc.s    V_5
      IL_0030:  ldloc.s    V_5
      IL_0032:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0037:  callvirt   instance int32 [mscorlib]System.String::get_Length()
      IL_003c:  brfalse.s  IL_005a

      IL_003e:  ldloc.s    V_5
      IL_0040:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_0045:  ldarg.0
      IL_0046:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_004b:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_0050:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_0055:  brfalse    IL_0100

      IL_005a:  ldstr      "Attributes"
      IL_005f:  ldstr      "TableOfContents"
      IL_0064:  ldstr      "TargetCollection"
      IL_0069:  ldstr      "TargetTableOfContents"
      IL_006e:  leave.s    IL_0070

      IL_0070:  ldloc.s    V_5
      IL_0072:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_LocalName()
      IL_0077:  dup
      IL_0078:  stloc.s    V_9
      IL_007a:  brfalse.s  IL_00f0

      IL_007c:  ldloc.s    V_9
      IL_007e:  call       string [mscorlib]System.String::IsInterned(string)
      IL_0083:  stloc.s    V_9
      IL_0085:  ldloc.s    V_9
      IL_0087:  ldstr      "Attributes"
      IL_008c:  beq.s      IL_00ab

      IL_008e:  ldloc.s    V_9
      IL_0090:  ldstr      "TableOfContents"
      IL_0095:  beq.s      IL_00bc

      IL_0097:  ldloc.s    V_9
      IL_0099:  ldstr      "TargetCollection"
      IL_009e:  beq.s      IL_00cd

      IL_00a0:  ldloc.s    V_9
      IL_00a2:  ldstr      "TargetTableOfContents"
      IL_00a7:  beq.s      IL_00df

      IL_00a9:  br.s       IL_00f0

      IL_00ab:  ldarg.0
      IL_00ac:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00b1:  ldloc.0
      IL_00b2:  ldloc.s    V_5
      IL_00b4:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00b9:  stloc.1
      IL_00ba:  br.s       IL_010e

      IL_00bc:  ldarg.0
      IL_00bd:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00c2:  ldloc.0
      IL_00c3:  ldloc.s    V_5
      IL_00c5:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00ca:  stloc.2
      IL_00cb:  br.s       IL_010e

      IL_00cd:  ldarg.0
      IL_00ce:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00d3:  ldloc.0
      IL_00d4:  ldloc.s    V_5
      IL_00d6:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00db:  stloc.s    V_4
      IL_00dd:  br.s       IL_010e

      IL_00df:  ldarg.0
      IL_00e0:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00e5:  ldloc.0
      IL_00e6:  ldloc.s    V_5
      IL_00e8:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::GetAttributeIdentifierValue(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_00ed:  stloc.3
      IL_00ee:  br.s       IL_010e

      IL_00f0:  ldarg.0
      IL_00f1:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_00f6:  ldloc.0
      IL_00f7:  ldloc.s    V_5
      IL_00f9:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                    class [System.Xml]System.Xml.XmlAttribute)
      IL_00fe:  br.s       IL_010e

      IL_0100:  ldarg.0
      IL_0101:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0106:  ldloc.0
      IL_0107:  ldloc.s    V_5
      IL_0109:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                              class [System.Xml]System.Xml.XmlAttribute)
      IL_010e:  ldloc.s    V_8
      IL_0110:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_0115:  brtrue     IL_0022

      IL_011a:  leave.s    IL_0131

    }  // end .try
    finally
    {
      IL_011c:  ldloc.s    V_8
      IL_011e:  isinst     [mscorlib]System.IDisposable
      IL_0123:  stloc.s    V_10
      IL_0125:  ldloc.s    V_10
      IL_0127:  brfalse.s  IL_0130

      IL_0129:  ldloc.s    V_10
      IL_012b:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0130:  endfinally
    }  // end handler
    IL_0131:  ldloc.s    V_4
    IL_0133:  brtrue.s   IL_0151

    IL_0135:  ldarg.0
    IL_0136:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_013b:  ldloc.0
    IL_013c:  ldarg.1
    IL_013d:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_Name()
    IL_0142:  ldstr      "TargetCollection"
    IL_0147:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixErrorEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixErrors::ExpectedAttribute(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                string,
                                                                                                                                                                string)
    IL_014c:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
    IL_0151:  ldarg.1
    IL_0152:  callvirt   instance class [System.Xml]System.Xml.XmlNodeList [System.Xml]System.Xml.XmlNode::get_ChildNodes()
    IL_0157:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [System.Xml]System.Xml.XmlNodeList::GetEnumerator()
    IL_015c:  stloc.s    V_8
    .try
    {
      IL_015e:  br.s       IL_01af

      IL_0160:  ldloc.s    V_8
      IL_0162:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0167:  castclass  [System.Xml]System.Xml.XmlNode
      IL_016c:  stloc.s    V_6
      IL_016e:  ldc.i4.1
      IL_016f:  ldloc.s    V_6
      IL_0171:  callvirt   instance valuetype [System.Xml]System.Xml.XmlNodeType [System.Xml]System.Xml.XmlNode::get_NodeType()
      IL_0176:  bne.un.s   IL_01af

      IL_0178:  ldloc.s    V_6
      IL_017a:  callvirt   instance string [System.Xml]System.Xml.XmlNode::get_NamespaceURI()
      IL_017f:  ldarg.0
      IL_0180:  ldfld      class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::schema
      IL_0185:  callvirt   instance string [System.Xml]System.Xml.Schema.XmlSchema::get_TargetNamespace()
      IL_018a:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_018f:  brfalse.s  IL_01a1

      IL_0191:  ldarg.0
      IL_0192:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_0197:  ldarg.1
      IL_0198:  ldloc.s    V_6
      IL_019a:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnexpectedElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                  class [System.Xml]System.Xml.XmlNode)
      IL_019f:  br.s       IL_01af

      IL_01a1:  ldarg.0
      IL_01a2:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
      IL_01a7:  ldarg.1
      IL_01a8:  ldloc.s    V_6
      IL_01aa:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::UnsupportedExtensionElement(class [System.Xml]System.Xml.XmlNode,
                                                                                                                            class [System.Xml]System.Xml.XmlNode)
      IL_01af:  ldloc.s    V_8
      IL_01b1:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_01b6:  brtrue.s   IL_0160

      IL_01b8:  leave.s    IL_01cf

    }  // end .try
    finally
    {
      IL_01ba:  ldloc.s    V_8
      IL_01bc:  isinst     [mscorlib]System.IDisposable
      IL_01c1:  stloc.s    V_10
      IL_01c3:  ldloc.s    V_10
      IL_01c5:  brfalse.s  IL_01ce

      IL_01c7:  ldloc.s    V_10
      IL_01c9:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_01ce:  endfinally
    }  // end handler
    IL_01cf:  ldarg.0
    IL_01d0:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_01d5:  callvirt   instance bool [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::get_EncounteredError()
    IL_01da:  brtrue.s   IL_021d

    IL_01dc:  ldarg.0
    IL_01dd:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension::get_Core()
    IL_01e2:  ldloc.0
    IL_01e3:  ldstr      "HelpPlugin"
    IL_01e8:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Row [wix]Microsoft.Tools.WindowsInstallerXml.CompilerCore::CreateRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                      string)
    IL_01ed:  stloc.s    V_7
    IL_01ef:  ldloc.s    V_7
    IL_01f1:  ldc.i4.0
    IL_01f2:  ldarg.2
    IL_01f3:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_01f8:  ldloc.s    V_7
    IL_01fa:  ldc.i4.1
    IL_01fb:  ldloc.s    V_4
    IL_01fd:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0202:  ldloc.s    V_7
    IL_0204:  ldc.i4.2
    IL_0205:  ldloc.2
    IL_0206:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_020b:  ldloc.s    V_7
    IL_020d:  ldc.i4.3
    IL_020e:  ldloc.1
    IL_020f:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_0214:  ldloc.s    V_7
    IL_0216:  ldc.i4.4
    IL_0217:  ldloc.3
    IL_0218:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Row::set_Item(int32,
                                                                                              object)
    IL_021d:  ret
  } // end of method VSCompiler::ParsePlugCollectionIntoElement

  .property instance class [System.Xml]System.Xml.Schema.XmlSchema
          Schema()
  {
    .get instance class [System.Xml]System.Xml.Schema.XmlSchema Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::get_Schema()
  } // end of property VSCompiler::Schema
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler

.class public auto ansi sealed beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler
       extends [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension
{
  .method public hidebysig virtual instance void 
          DecompileTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table table) cil managed
  {
    // Code size       155 (0x9b)
    .maxstack  6
    .locals init (string V_0)
    IL_0000:  ldstr      "HelpFile"
    IL_0005:  ldstr      "HelpFileToNamespace"
    IL_000a:  ldstr      "HelpFilter"
    IL_000f:  ldstr      "HelpFilterToNamespace"
    IL_0014:  ldstr      "HelpNamespace"
    IL_0019:  ldstr      "HelpPlugin"
    IL_001e:  leave.s    IL_0020

    IL_0020:  ldarg.1
    IL_0021:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Name()
    IL_0026:  dup
    IL_0027:  stloc.0
    IL_0028:  brfalse.s  IL_0093

    IL_002a:  ldloc.0
    IL_002b:  call       string [mscorlib]System.String::IsInterned(string)
    IL_0030:  stloc.0
    IL_0031:  ldloc.0
    IL_0032:  ldstr      "HelpFile"
    IL_0037:  beq.s      IL_0063

    IL_0039:  ldloc.0
    IL_003a:  ldstr      "HelpFileToNamespace"
    IL_003f:  beq.s      IL_006b

    IL_0041:  ldloc.0
    IL_0042:  ldstr      "HelpFilter"
    IL_0047:  beq.s      IL_0073

    IL_0049:  ldloc.0
    IL_004a:  ldstr      "HelpFilterToNamespace"
    IL_004f:  beq.s      IL_007b

    IL_0051:  ldloc.0
    IL_0052:  ldstr      "HelpNamespace"
    IL_0057:  beq.s      IL_0083

    IL_0059:  ldloc.0
    IL_005a:  ldstr      "HelpPlugin"
    IL_005f:  beq.s      IL_008b

    IL_0061:  br.s       IL_0093

    IL_0063:  ldarg.0
    IL_0064:  ldarg.1
    IL_0065:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler::DecompileHelpFileTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table)
    IL_006a:  ret

    IL_006b:  ldarg.0
    IL_006c:  ldarg.1
    IL_006d:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler::DecompileHelpFileToNamespaceTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table)
    IL_0072:  ret

    IL_0073:  ldarg.0
    IL_0074:  ldarg.1
    IL_0075:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler::DecompileHelpFilterTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table)
    IL_007a:  ret

    IL_007b:  ldarg.0
    IL_007c:  ldarg.1
    IL_007d:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler::DecompileHelpFilterToNamespaceTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table)
    IL_0082:  ret

    IL_0083:  ldarg.0
    IL_0084:  ldarg.1
    IL_0085:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler::DecompileHelpNamespaceTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table)
    IL_008a:  ret

    IL_008b:  ldarg.0
    IL_008c:  ldarg.1
    IL_008d:  call       instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler::DecompileHelpPluginTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table)
    IL_0092:  ret

    IL_0093:  ldarg.0
    IL_0094:  ldarg.1
    IL_0095:  call       instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::DecompileTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table)
    IL_009a:  ret
  } // end of method VSDecompiler::DecompileTable

  .method private hidebysig instance void 
          DecompileHelpFileTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table table) cil managed
  {
    // Code size       359 (0x167)
    .maxstack  8
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_0,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile V_1,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.File V_2,
             class [mscorlib]System.Collections.IEnumerator V_3,
             string[] V_4,
             class [mscorlib]System.IDisposable V_5)
    IL_0000:  ldarg.1
    IL_0001:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Rows()
    IL_0006:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection::GetEnumerator()
    IL_000b:  stloc.3
    .try
    {
      IL_000c:  br         IL_0145

      IL_0011:  ldloc.3
      IL_0012:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0017:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Row
      IL_001c:  stloc.0
      IL_001d:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::.ctor()
      IL_0022:  stloc.1
      IL_0023:  ldloc.1
      IL_0024:  ldloc.0
      IL_0025:  ldc.i4.0
      IL_0026:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_002b:  castclass  [mscorlib]System.String
      IL_0030:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Id(string)
      IL_0035:  ldloc.1
      IL_0036:  ldloc.0
      IL_0037:  ldc.i4.1
      IL_0038:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_003d:  castclass  [mscorlib]System.String
      IL_0042:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Name(string)
      IL_0047:  ldloc.0
      IL_0048:  ldc.i4.2
      IL_0049:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_004e:  brfalse.s  IL_0063

      IL_0050:  ldloc.1
      IL_0051:  ldloc.0
      IL_0052:  ldc.i4.2
      IL_0053:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0058:  unbox      [mscorlib]System.Int32
      IL_005d:  ldind.i4
      IL_005e:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Language(int32)
      IL_0063:  ldloc.0
      IL_0064:  ldc.i4.4
      IL_0065:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_006a:  brfalse.s  IL_007e

      IL_006c:  ldloc.1
      IL_006d:  ldloc.0
      IL_006e:  ldc.i4.4
      IL_006f:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0074:  castclass  [mscorlib]System.String
      IL_0079:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Index(string)
      IL_007e:  ldloc.0
      IL_007f:  ldc.i4.5
      IL_0080:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0085:  brfalse.s  IL_0099

      IL_0087:  ldloc.1
      IL_0088:  ldloc.0
      IL_0089:  ldc.i4.5
      IL_008a:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_008f:  castclass  [mscorlib]System.String
      IL_0094:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Search(string)
      IL_0099:  ldloc.0
      IL_009a:  ldc.i4.6
      IL_009b:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00a0:  brfalse.s  IL_00b4

      IL_00a2:  ldloc.1
      IL_00a3:  ldloc.0
      IL_00a4:  ldc.i4.6
      IL_00a5:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00aa:  castclass  [mscorlib]System.String
      IL_00af:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_AttributeIndex(string)
      IL_00b4:  ldloc.0
      IL_00b5:  ldc.i4.7
      IL_00b6:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00bb:  brfalse.s  IL_00cf

      IL_00bd:  ldloc.1
      IL_00be:  ldloc.0
      IL_00bf:  ldc.i4.7
      IL_00c0:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00c5:  castclass  [mscorlib]System.String
      IL_00ca:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_SampleLocation(string)
      IL_00cf:  ldarg.0
      IL_00d0:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_00d5:  ldstr      "File"
      IL_00da:  ldc.i4.1
      IL_00db:  newarr     [mscorlib]System.String
      IL_00e0:  stloc.s    V_4
      IL_00e2:  ldloc.s    V_4
      IL_00e4:  ldc.i4.0
      IL_00e5:  ldloc.0
      IL_00e6:  ldc.i4.3
      IL_00e7:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00ec:  castclass  [mscorlib]System.String
      IL_00f1:  stelem.ref
      IL_00f2:  ldloc.s    V_4
      IL_00f4:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::GetIndexedElement(string,
                                                                                                                                                                                       string[])
      IL_00f9:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.File
      IL_00fe:  stloc.2
      IL_00ff:  ldloc.2
      IL_0100:  brfalse.s  IL_010b

      IL_0102:  ldloc.2
      IL_0103:  ldloc.1
      IL_0104:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.File::AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
      IL_0109:  br.s       IL_0145

      IL_010b:  ldarg.0
      IL_010c:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_0111:  ldloc.0
      IL_0112:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_SourceLineNumbers()
      IL_0117:  ldarg.1
      IL_0118:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Name()
      IL_011d:  ldloc.0
      IL_011e:  ldc.i4.s   47
      IL_0120:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Row::GetPrimaryKey(char)
      IL_0125:  ldstr      "File_HxS"
      IL_012a:  ldloc.0
      IL_012b:  ldc.i4.3
      IL_012c:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0131:  castclass  [mscorlib]System.String
      IL_0136:  ldstr      "File"
      IL_013b:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixWarningEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixWarnings::ExpectedForeignRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string)
      IL_0140:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
      IL_0145:  ldloc.3
      IL_0146:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_014b:  brtrue     IL_0011

      IL_0150:  leave.s    IL_0166

    }  // end .try
    finally
    {
      IL_0152:  ldloc.3
      IL_0153:  isinst     [mscorlib]System.IDisposable
      IL_0158:  stloc.s    V_5
      IL_015a:  ldloc.s    V_5
      IL_015c:  brfalse.s  IL_0165

      IL_015e:  ldloc.s    V_5
      IL_0160:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0165:  endfinally
    }  // end handler
    IL_0166:  ret
  } // end of method VSDecompiler::DecompileHelpFileTable

  .method private hidebysig instance void 
          DecompileHelpFileToNamespaceTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table table) cil managed
  {
    // Code size       205 (0xcd)
    .maxstack  8
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_0,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef V_1,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection V_2,
             class [mscorlib]System.Collections.IEnumerator V_3,
             string[] V_4,
             class [mscorlib]System.IDisposable V_5)
    IL_0000:  ldarg.1
    IL_0001:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Rows()
    IL_0006:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection::GetEnumerator()
    IL_000b:  stloc.3
    .try
    {
      IL_000c:  br         IL_00ab

      IL_0011:  ldloc.3
      IL_0012:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0017:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Row
      IL_001c:  stloc.0
      IL_001d:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::.ctor()
      IL_0022:  stloc.1
      IL_0023:  ldloc.1
      IL_0024:  ldloc.0
      IL_0025:  ldc.i4.0
      IL_0026:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_002b:  castclass  [mscorlib]System.String
      IL_0030:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::set_Id(string)
      IL_0035:  ldarg.0
      IL_0036:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_003b:  ldstr      "HelpNamespace"
      IL_0040:  ldc.i4.1
      IL_0041:  newarr     [mscorlib]System.String
      IL_0046:  stloc.s    V_4
      IL_0048:  ldloc.s    V_4
      IL_004a:  ldc.i4.0
      IL_004b:  ldloc.0
      IL_004c:  ldc.i4.1
      IL_004d:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0052:  castclass  [mscorlib]System.String
      IL_0057:  stelem.ref
      IL_0058:  ldloc.s    V_4
      IL_005a:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::GetIndexedElement(string,
                                                                                                                                                                                       string[])
      IL_005f:  castclass  Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection
      IL_0064:  stloc.2
      IL_0065:  ldloc.2
      IL_0066:  brfalse.s  IL_0071

      IL_0068:  ldloc.2
      IL_0069:  ldloc.1
      IL_006a:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
      IL_006f:  br.s       IL_00ab

      IL_0071:  ldarg.0
      IL_0072:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_0077:  ldloc.0
      IL_0078:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_SourceLineNumbers()
      IL_007d:  ldarg.1
      IL_007e:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Name()
      IL_0083:  ldloc.0
      IL_0084:  ldc.i4.s   47
      IL_0086:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Row::GetPrimaryKey(char)
      IL_008b:  ldstr      "HelpNamespace_"
      IL_0090:  ldloc.0
      IL_0091:  ldc.i4.1
      IL_0092:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0097:  castclass  [mscorlib]System.String
      IL_009c:  ldstr      "HelpNamespace"
      IL_00a1:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixWarningEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixWarnings::ExpectedForeignRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string)
      IL_00a6:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
      IL_00ab:  ldloc.3
      IL_00ac:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00b1:  brtrue     IL_0011

      IL_00b6:  leave.s    IL_00cc

    }  // end .try
    finally
    {
      IL_00b8:  ldloc.3
      IL_00b9:  isinst     [mscorlib]System.IDisposable
      IL_00be:  stloc.s    V_5
      IL_00c0:  ldloc.s    V_5
      IL_00c2:  brfalse.s  IL_00cb

      IL_00c4:  ldloc.s    V_5
      IL_00c6:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_00cb:  endfinally
    }  // end handler
    IL_00cc:  ret
  } // end of method VSDecompiler::DecompileHelpFileToNamespaceTable

  .method private hidebysig instance void 
          DecompileHelpFilterTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table table) cil managed
  {
    // Code size       140 (0x8c)
    .maxstack  4
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_0,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter V_1,
             class [mscorlib]System.Collections.IEnumerator V_2,
             class [mscorlib]System.IDisposable V_3)
    IL_0000:  ldarg.1
    IL_0001:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Rows()
    IL_0006:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection::GetEnumerator()
    IL_000b:  stloc.2
    .try
    {
      IL_000c:  br.s       IL_0070

      IL_000e:  ldloc.2
      IL_000f:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0014:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Row
      IL_0019:  stloc.0
      IL_001a:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::.ctor()
      IL_001f:  stloc.1
      IL_0020:  ldloc.1
      IL_0021:  ldloc.0
      IL_0022:  ldc.i4.0
      IL_0023:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0028:  castclass  [mscorlib]System.String
      IL_002d:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::set_Id(string)
      IL_0032:  ldloc.1
      IL_0033:  ldloc.0
      IL_0034:  ldc.i4.1
      IL_0035:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_003a:  castclass  [mscorlib]System.String
      IL_003f:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::set_Name(string)
      IL_0044:  ldloc.0
      IL_0045:  ldc.i4.2
      IL_0046:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_004b:  brfalse.s  IL_005f

      IL_004d:  ldloc.1
      IL_004e:  ldloc.0
      IL_004f:  ldc.i4.2
      IL_0050:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0055:  castclass  [mscorlib]System.String
      IL_005a:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::set_FilterDefinition(string)
      IL_005f:  ldarg.0
      IL_0060:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_0065:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.IParentElement [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::get_RootElement()
      IL_006a:  ldloc.1
      IL_006b:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.IParentElement::AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
      IL_0070:  ldloc.2
      IL_0071:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_0076:  brtrue.s   IL_000e

      IL_0078:  leave.s    IL_008b

    }  // end .try
    finally
    {
      IL_007a:  ldloc.2
      IL_007b:  isinst     [mscorlib]System.IDisposable
      IL_0080:  stloc.3
      IL_0081:  ldloc.3
      IL_0082:  brfalse.s  IL_008a

      IL_0084:  ldloc.3
      IL_0085:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_008a:  endfinally
    }  // end handler
    IL_008b:  ret
  } // end of method VSDecompiler::DecompileHelpFilterTable

  .method private hidebysig instance void 
          DecompileHelpFilterToNamespaceTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table table) cil managed
  {
    // Code size       205 (0xcd)
    .maxstack  8
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_0,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef V_1,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection V_2,
             class [mscorlib]System.Collections.IEnumerator V_3,
             string[] V_4,
             class [mscorlib]System.IDisposable V_5)
    IL_0000:  ldarg.1
    IL_0001:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Rows()
    IL_0006:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection::GetEnumerator()
    IL_000b:  stloc.3
    .try
    {
      IL_000c:  br         IL_00ab

      IL_0011:  ldloc.3
      IL_0012:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0017:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Row
      IL_001c:  stloc.0
      IL_001d:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::.ctor()
      IL_0022:  stloc.1
      IL_0023:  ldloc.1
      IL_0024:  ldloc.0
      IL_0025:  ldc.i4.0
      IL_0026:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_002b:  castclass  [mscorlib]System.String
      IL_0030:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::set_Id(string)
      IL_0035:  ldarg.0
      IL_0036:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_003b:  ldstr      "HelpNamespace"
      IL_0040:  ldc.i4.1
      IL_0041:  newarr     [mscorlib]System.String
      IL_0046:  stloc.s    V_4
      IL_0048:  ldloc.s    V_4
      IL_004a:  ldc.i4.0
      IL_004b:  ldloc.0
      IL_004c:  ldc.i4.1
      IL_004d:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0052:  castclass  [mscorlib]System.String
      IL_0057:  stelem.ref
      IL_0058:  ldloc.s    V_4
      IL_005a:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::GetIndexedElement(string,
                                                                                                                                                                                       string[])
      IL_005f:  castclass  Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection
      IL_0064:  stloc.2
      IL_0065:  ldloc.2
      IL_0066:  brfalse.s  IL_0071

      IL_0068:  ldloc.2
      IL_0069:  ldloc.1
      IL_006a:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
      IL_006f:  br.s       IL_00ab

      IL_0071:  ldarg.0
      IL_0072:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_0077:  ldloc.0
      IL_0078:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_SourceLineNumbers()
      IL_007d:  ldarg.1
      IL_007e:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Name()
      IL_0083:  ldloc.0
      IL_0084:  ldc.i4.s   47
      IL_0086:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Row::GetPrimaryKey(char)
      IL_008b:  ldstr      "HelpNamespace_"
      IL_0090:  ldloc.0
      IL_0091:  ldc.i4.1
      IL_0092:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0097:  castclass  [mscorlib]System.String
      IL_009c:  ldstr      "HelpNamespace"
      IL_00a1:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixWarningEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixWarnings::ExpectedForeignRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string)
      IL_00a6:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
      IL_00ab:  ldloc.3
      IL_00ac:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00b1:  brtrue     IL_0011

      IL_00b6:  leave.s    IL_00cc

    }  // end .try
    finally
    {
      IL_00b8:  ldloc.3
      IL_00b9:  isinst     [mscorlib]System.IDisposable
      IL_00be:  stloc.s    V_5
      IL_00c0:  ldloc.s    V_5
      IL_00c2:  brfalse.s  IL_00cb

      IL_00c4:  ldloc.s    V_5
      IL_00c6:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_00cb:  endfinally
    }  // end handler
    IL_00cc:  ret
  } // end of method VSDecompiler::DecompileHelpFilterToNamespaceTable

  .method private hidebysig instance void 
          DecompileHelpNamespaceTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table table) cil managed
  {
    // Code size       263 (0x107)
    .maxstack  8
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_0,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection V_1,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.File V_2,
             class [mscorlib]System.Collections.IEnumerator V_3,
             string[] V_4,
             class [mscorlib]System.IDisposable V_5)
    IL_0000:  ldarg.1
    IL_0001:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Rows()
    IL_0006:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection::GetEnumerator()
    IL_000b:  stloc.3
    .try
    {
      IL_000c:  br         IL_00e5

      IL_0011:  ldloc.3
      IL_0012:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0017:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Row
      IL_001c:  stloc.0
      IL_001d:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::.ctor()
      IL_0022:  stloc.1
      IL_0023:  ldloc.1
      IL_0024:  ldloc.0
      IL_0025:  ldc.i4.0
      IL_0026:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_002b:  castclass  [mscorlib]System.String
      IL_0030:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::set_Id(string)
      IL_0035:  ldloc.1
      IL_0036:  ldloc.0
      IL_0037:  ldc.i4.1
      IL_0038:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_003d:  castclass  [mscorlib]System.String
      IL_0042:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::set_Name(string)
      IL_0047:  ldloc.0
      IL_0048:  ldc.i4.3
      IL_0049:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_004e:  brfalse.s  IL_0062

      IL_0050:  ldloc.1
      IL_0051:  ldloc.0
      IL_0052:  ldc.i4.3
      IL_0053:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0058:  castclass  [mscorlib]System.String
      IL_005d:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::set_Description(string)
      IL_0062:  ldarg.0
      IL_0063:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_0068:  ldstr      "File"
      IL_006d:  ldc.i4.1
      IL_006e:  newarr     [mscorlib]System.String
      IL_0073:  stloc.s    V_4
      IL_0075:  ldloc.s    V_4
      IL_0077:  ldc.i4.0
      IL_0078:  ldloc.0
      IL_0079:  ldc.i4.2
      IL_007a:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_007f:  castclass  [mscorlib]System.String
      IL_0084:  stelem.ref
      IL_0085:  ldloc.s    V_4
      IL_0087:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::GetIndexedElement(string,
                                                                                                                                                                                       string[])
      IL_008c:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.File
      IL_0091:  stloc.2
      IL_0092:  ldloc.2
      IL_0093:  brfalse.s  IL_009e

      IL_0095:  ldloc.2
      IL_0096:  ldloc.1
      IL_0097:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.File::AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
      IL_009c:  br.s       IL_00d8

      IL_009e:  ldarg.0
      IL_009f:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_00a4:  ldloc.0
      IL_00a5:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_SourceLineNumbers()
      IL_00aa:  ldarg.1
      IL_00ab:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Name()
      IL_00b0:  ldloc.0
      IL_00b1:  ldc.i4.s   47
      IL_00b3:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Row::GetPrimaryKey(char)
      IL_00b8:  ldstr      "File_Collection"
      IL_00bd:  ldloc.0
      IL_00be:  ldc.i4.2
      IL_00bf:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00c4:  castclass  [mscorlib]System.String
      IL_00c9:  ldstr      "File"
      IL_00ce:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixWarningEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixWarnings::ExpectedForeignRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string)
      IL_00d3:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
      IL_00d8:  ldarg.0
      IL_00d9:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_00de:  ldloc.0
      IL_00df:  ldloc.1
      IL_00e0:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::IndexElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Row,
                                                                                                               class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
      IL_00e5:  ldloc.3
      IL_00e6:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00eb:  brtrue     IL_0011

      IL_00f0:  leave.s    IL_0106

    }  // end .try
    finally
    {
      IL_00f2:  ldloc.3
      IL_00f3:  isinst     [mscorlib]System.IDisposable
      IL_00f8:  stloc.s    V_5
      IL_00fa:  ldloc.s    V_5
      IL_00fc:  brfalse.s  IL_0105

      IL_00fe:  ldloc.s    V_5
      IL_0100:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0105:  endfinally
    }  // end handler
    IL_0106:  ret
  } // end of method VSDecompiler::DecompileHelpNamespaceTable

  .method private hidebysig instance void 
          DecompileHelpPluginTable(class [wix]Microsoft.Tools.WindowsInstallerXml.Table table) cil managed
  {
    // Code size       286 (0x11e)
    .maxstack  8
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Row V_0,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto V_1,
             class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection V_2,
             class [mscorlib]System.Collections.IEnumerator V_3,
             string[] V_4,
             class [mscorlib]System.IDisposable V_5)
    IL_0000:  ldarg.1
    IL_0001:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Rows()
    IL_0006:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.RowCollection::GetEnumerator()
    IL_000b:  stloc.3
    .try
    {
      IL_000c:  br         IL_00fc

      IL_0011:  ldloc.3
      IL_0012:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_0017:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Row
      IL_001c:  stloc.0
      IL_001d:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::.ctor()
      IL_0022:  stloc.1
      IL_0023:  ldloc.1
      IL_0024:  ldloc.0
      IL_0025:  ldc.i4.1
      IL_0026:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_002b:  castclass  [mscorlib]System.String
      IL_0030:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_TargetCollection(string)
      IL_0035:  ldloc.0
      IL_0036:  ldc.i4.2
      IL_0037:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_003c:  brfalse.s  IL_0050

      IL_003e:  ldloc.1
      IL_003f:  ldloc.0
      IL_0040:  ldc.i4.2
      IL_0041:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0046:  castclass  [mscorlib]System.String
      IL_004b:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_TableOfContents(string)
      IL_0050:  ldloc.0
      IL_0051:  ldc.i4.3
      IL_0052:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0057:  brfalse.s  IL_006b

      IL_0059:  ldloc.1
      IL_005a:  ldloc.0
      IL_005b:  ldc.i4.3
      IL_005c:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0061:  castclass  [mscorlib]System.String
      IL_0066:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_Attributes(string)
      IL_006b:  ldloc.0
      IL_006c:  ldc.i4.4
      IL_006d:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_0072:  brfalse.s  IL_0086

      IL_0074:  ldloc.1
      IL_0075:  ldloc.0
      IL_0076:  ldc.i4.4
      IL_0077:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_007c:  castclass  [mscorlib]System.String
      IL_0081:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_TargetTableOfContents(string)
      IL_0086:  ldarg.0
      IL_0087:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_008c:  ldstr      "HelpNamespace"
      IL_0091:  ldc.i4.1
      IL_0092:  newarr     [mscorlib]System.String
      IL_0097:  stloc.s    V_4
      IL_0099:  ldloc.s    V_4
      IL_009b:  ldc.i4.0
      IL_009c:  ldloc.0
      IL_009d:  ldc.i4.0
      IL_009e:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00a3:  castclass  [mscorlib]System.String
      IL_00a8:  stelem.ref
      IL_00a9:  ldloc.s    V_4
      IL_00ab:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::GetIndexedElement(string,
                                                                                                                                                                                       string[])
      IL_00b0:  castclass  Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection
      IL_00b5:  stloc.2
      IL_00b6:  ldloc.2
      IL_00b7:  brfalse.s  IL_00c2

      IL_00b9:  ldloc.2
      IL_00ba:  ldloc.1
      IL_00bb:  callvirt   instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
      IL_00c0:  br.s       IL_00fc

      IL_00c2:  ldarg.0
      IL_00c3:  call       instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::get_Core()
      IL_00c8:  ldloc.0
      IL_00c9:  callvirt   instance class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_SourceLineNumbers()
      IL_00ce:  ldarg.1
      IL_00cf:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Table::get_Name()
      IL_00d4:  ldloc.0
      IL_00d5:  ldc.i4.s   47
      IL_00d7:  callvirt   instance string [wix]Microsoft.Tools.WindowsInstallerXml.Row::GetPrimaryKey(char)
      IL_00dc:  ldstr      "HelpNamespace_"
      IL_00e1:  ldloc.0
      IL_00e2:  ldc.i4.0
      IL_00e3:  callvirt   instance object [wix]Microsoft.Tools.WindowsInstallerXml.Row::get_Item(int32)
      IL_00e8:  castclass  [mscorlib]System.String
      IL_00ed:  ldstr      "HelpNamespace"
      IL_00f2:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.WixWarningEventArgs [wix]Microsoft.Tools.WindowsInstallerXml.WixWarnings::ExpectedForeignRow(class [wix]Microsoft.Tools.WindowsInstallerXml.SourceLineNumberCollection,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string,
                                                                                                                                                                       string)
      IL_00f7:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerCore::OnMessage(class [wix]Microsoft.Tools.WindowsInstallerXml.MessageEventArgs)
      IL_00fc:  ldloc.3
      IL_00fd:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_0102:  brtrue     IL_0011

      IL_0107:  leave.s    IL_011d

    }  // end .try
    finally
    {
      IL_0109:  ldloc.3
      IL_010a:  isinst     [mscorlib]System.IDisposable
      IL_010f:  stloc.s    V_5
      IL_0111:  ldloc.s    V_5
      IL_0113:  brfalse.s  IL_011c

      IL_0115:  ldloc.s    V_5
      IL_0117:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_011c:  endfinally
    }  // end handler
    IL_011d:  ret
  } // end of method VSDecompiler::DecompileHelpPluginTable

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension::.ctor()
    IL_0006:  ret
  } // end of method VSDecompiler::.ctor

} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler

.class public auto ansi sealed beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension
       extends [wix]Microsoft.Tools.WindowsInstallerXml.WixExtension
{
  .field private class Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler compilerExtension
  .field private class Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler decompilerExtension
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Library 'library'
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection tableDefinitions
  .method public hidebysig specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension 
          get_CompilerExtension() cil managed
  {
    // Code size       26 (0x1a)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::compilerExtension
    IL_0006:  brtrue.s   IL_0013

    IL_0008:  ldarg.0
    IL_0009:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler::.ctor()
    IL_000e:  stfld      class Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::compilerExtension
    IL_0013:  ldarg.0
    IL_0014:  ldfld      class Microsoft.Tools.WindowsInstallerXml.Extensions.VSCompiler Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::compilerExtension
    IL_0019:  ret
  } // end of method VSExtension::get_CompilerExtension

  .method public hidebysig specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension 
          get_DecompilerExtension() cil managed
  {
    // Code size       26 (0x1a)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::decompilerExtension
    IL_0006:  brtrue.s   IL_0013

    IL_0008:  ldarg.0
    IL_0009:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler::.ctor()
    IL_000e:  stfld      class Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::decompilerExtension
    IL_0013:  ldarg.0
    IL_0014:  ldfld      class Microsoft.Tools.WindowsInstallerXml.Extensions.VSDecompiler Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::decompilerExtension
    IL_0019:  ret
  } // end of method VSExtension::get_DecompilerExtension

  .method public hidebysig specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection 
          get_TableDefinitions() cil managed
  {
    // Code size       36 (0x24)
    .maxstack  3
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::tableDefinitions
    IL_0006:  brtrue.s   IL_001d

    IL_0008:  ldarg.0
    IL_0009:  call       class [mscorlib]System.Reflection.Assembly [mscorlib]System.Reflection.Assembly::GetExecutingAssembly()
    IL_000e:  ldstr      "Microsoft.Tools.WindowsInstallerXml.Extensions.Dat"
    + "a.tables.xml"
    IL_0013:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection [wix]Microsoft.Tools.WindowsInstallerXml.WixExtension::LoadTableDefinitionHelper(class [mscorlib]System.Reflection.Assembly,
                                                                                                                                                                                   string)
    IL_0018:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::tableDefinitions
    IL_001d:  ldarg.0
    IL_001e:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::tableDefinitions
    IL_0023:  ret
  } // end of method VSExtension::get_TableDefinitions

  .method public hidebysig virtual instance class [wix]Microsoft.Tools.WindowsInstallerXml.Library 
          GetLibrary(class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection tableDefinitions) cil managed
  {
    // Code size       37 (0x25)
    .maxstack  4
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Library Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::'library'
    IL_0006:  brtrue.s   IL_001e

    IL_0008:  ldarg.0
    IL_0009:  call       class [mscorlib]System.Reflection.Assembly [mscorlib]System.Reflection.Assembly::GetExecutingAssembly()
    IL_000e:  ldstr      "Microsoft.Tools.WindowsInstallerXml.Extensions.Dat"
    + "a.vs.wixlib"
    IL_0013:  ldarg.1
    IL_0014:  call       class [wix]Microsoft.Tools.WindowsInstallerXml.Library [wix]Microsoft.Tools.WindowsInstallerXml.WixExtension::LoadLibraryHelper(class [mscorlib]System.Reflection.Assembly,
                                                                                                                                                         string,
                                                                                                                                                         class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection)
    IL_0019:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Library Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::'library'
    IL_001e:  ldarg.0
    IL_001f:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Library Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::'library'
    IL_0024:  ret
  } // end of method VSExtension::GetLibrary

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [wix]Microsoft.Tools.WindowsInstallerXml.WixExtension::.ctor()
    IL_0006:  ret
  } // end of method VSExtension::.ctor

  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension
          CompilerExtension()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.CompilerExtension Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::get_CompilerExtension()
  } // end of property VSExtension::CompilerExtension
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension
          DecompilerExtension()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.DecompilerExtension Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::get_DecompilerExtension()
  } // end of property VSExtension::DecompilerExtension
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection
          TableDefinitions()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.TableDefinitionCollection Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension::get_TableDefinitions()
  } // end of property VSExtension::TableDefinitions
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.VSExtension

.class public auto ansi beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter
       extends [mscorlib]System.Object
       implements [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes
{
  .field private string idField
  .field private bool idFieldSet
  .field private string filterDefinitionField
  .field private bool filterDefinitionFieldSet
  .field private string nameField
  .field private bool nameFieldSet
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement parentElement
  .method public hidebysig specialname instance string 
          get_Id() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::idField
    IL_0006:  ret
  } // end of method HelpFilter::get_Id

  .method public hidebysig specialname instance void 
          set_Id(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::idFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::idField
    IL_000e:  ret
  } // end of method HelpFilter::set_Id

  .method public hidebysig specialname instance string 
          get_FilterDefinition() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::filterDefinitionField
    IL_0006:  ret
  } // end of method HelpFilter::get_FilterDefinition

  .method public hidebysig specialname instance void 
          set_FilterDefinition(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::filterDefinitionFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::filterDefinitionField
    IL_000e:  ret
  } // end of method HelpFilter::set_FilterDefinition

  .method public hidebysig specialname instance string 
          get_Name() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::nameField
    IL_0006:  ret
  } // end of method HelpFilter::get_Name

  .method public hidebysig specialname instance void 
          set_Name(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::nameFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::nameField
    IL_000e:  ret
  } // end of method HelpFilter::set_Name

  .method public hidebysig newslot specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          get_ParentElement() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::parentElement
    IL_0006:  ret
  } // end of method HelpFilter::get_ParentElement

  .method public hidebysig newslot specialname virtual 
          instance void  set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 'value') cil managed
  {
    // Code size       8 (0x8)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::parentElement
    IL_0007:  ret
  } // end of method HelpFilter::set_ParentElement

  .method public hidebysig newslot virtual 
          instance void  OutputXml(class [System.Xml]System.Xml.XmlTextWriter writer) cil managed
  {
    // Code size       98 (0x62)
    .maxstack  3
    IL_0000:  ldarg.1
    IL_0001:  ldstr      "HelpFilter"
    IL_0006:  ldstr      "http://schemas.microsoft.com/wix/VSExtension"
    IL_000b:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteStartElement(string,
                                                                                           string)
    IL_0010:  ldarg.0
    IL_0011:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::idFieldSet
    IL_0016:  brfalse.s  IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      "Id"
    IL_001e:  ldarg.0
    IL_001f:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::idField
    IL_0024:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0029:  ldarg.0
    IL_002a:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::filterDefinitionFieldSet
    IL_002f:  brfalse.s  IL_0042

    IL_0031:  ldarg.1
    IL_0032:  ldstr      "FilterDefinition"
    IL_0037:  ldarg.0
    IL_0038:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::filterDefinitionField
    IL_003d:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0042:  ldarg.0
    IL_0043:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::nameFieldSet
    IL_0048:  brfalse.s  IL_005b

    IL_004a:  ldarg.1
    IL_004b:  ldstr      "Name"
    IL_0050:  ldarg.0
    IL_0051:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::nameField
    IL_0056:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_005b:  ldarg.1
    IL_005c:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteEndElement()
    IL_0061:  ret
  } // end of method HelpFilter::OutputXml

  .method private hidebysig newslot virtual final 
          instance void  Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute(string name,
                                                                                                   string 'value') cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes::SetAttribute
    // Code size       82 (0x52)
    .maxstack  2
    IL_0000:  ldstr      "Id"
    IL_0005:  ldarg.1
    IL_0006:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000b:  brfalse.s  IL_001b

    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::idField
    IL_0014:  ldarg.0
    IL_0015:  ldc.i4.1
    IL_0016:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::idFieldSet
    IL_001b:  ldstr      "FilterDefinition"
    IL_0020:  ldarg.1
    IL_0021:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0026:  brfalse.s  IL_0036

    IL_0028:  ldarg.0
    IL_0029:  ldarg.2
    IL_002a:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::filterDefinitionField
    IL_002f:  ldarg.0
    IL_0030:  ldc.i4.1
    IL_0031:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::filterDefinitionFieldSet
    IL_0036:  ldstr      "Name"
    IL_003b:  ldarg.1
    IL_003c:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0041:  brfalse.s  IL_0051

    IL_0043:  ldarg.0
    IL_0044:  ldarg.2
    IL_0045:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::nameField
    IL_004a:  ldarg.0
    IL_004b:  ldc.i4.1
    IL_004c:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::nameFieldSet
    IL_0051:  ret
  } // end of method HelpFilter::Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method HelpFilter::.ctor

  .property instance string Id()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::get_Id()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::set_Id(string)
  } // end of property HelpFilter::Id
  .property instance string FilterDefinition()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::get_FilterDefinition()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::set_FilterDefinition(string)
  } // end of property HelpFilter::FilterDefinition
  .property instance string Name()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::get_Name()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::set_Name(string)
  } // end of property HelpFilter::Name
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
          ParentElement()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::get_ParentElement()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
  } // end of property HelpFilter::ParentElement
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilter

.class public auto ansi beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto
       extends [mscorlib]System.Object
       implements [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes
{
  .field private string attributesField
  .field private bool attributesFieldSet
  .field private string tableOfContentsField
  .field private bool tableOfContentsFieldSet
  .field private string targetCollectionField
  .field private bool targetCollectionFieldSet
  .field private string targetTableOfContentsField
  .field private bool targetTableOfContentsFieldSet
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement parentElement
  .method public hidebysig specialname instance string 
          get_Attributes() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::attributesField
    IL_0006:  ret
  } // end of method PlugCollectionInto::get_Attributes

  .method public hidebysig specialname instance void 
          set_Attributes(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::attributesFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::attributesField
    IL_000e:  ret
  } // end of method PlugCollectionInto::set_Attributes

  .method public hidebysig specialname instance string 
          get_TableOfContents() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::tableOfContentsField
    IL_0006:  ret
  } // end of method PlugCollectionInto::get_TableOfContents

  .method public hidebysig specialname instance void 
          set_TableOfContents(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::tableOfContentsFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::tableOfContentsField
    IL_000e:  ret
  } // end of method PlugCollectionInto::set_TableOfContents

  .method public hidebysig specialname instance string 
          get_TargetCollection() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetCollectionField
    IL_0006:  ret
  } // end of method PlugCollectionInto::get_TargetCollection

  .method public hidebysig specialname instance void 
          set_TargetCollection(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetCollectionFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetCollectionField
    IL_000e:  ret
  } // end of method PlugCollectionInto::set_TargetCollection

  .method public hidebysig specialname instance string 
          get_TargetTableOfContents() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetTableOfContentsField
    IL_0006:  ret
  } // end of method PlugCollectionInto::get_TargetTableOfContents

  .method public hidebysig specialname instance void 
          set_TargetTableOfContents(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetTableOfContentsFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetTableOfContentsField
    IL_000e:  ret
  } // end of method PlugCollectionInto::set_TargetTableOfContents

  .method public hidebysig newslot specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          get_ParentElement() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::parentElement
    IL_0006:  ret
  } // end of method PlugCollectionInto::get_ParentElement

  .method public hidebysig newslot specialname virtual 
          instance void  set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 'value') cil managed
  {
    // Code size       8 (0x8)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::parentElement
    IL_0007:  ret
  } // end of method PlugCollectionInto::set_ParentElement

  .method public hidebysig newslot virtual 
          instance void  OutputXml(class [System.Xml]System.Xml.XmlTextWriter writer) cil managed
  {
    // Code size       123 (0x7b)
    .maxstack  3
    IL_0000:  ldarg.1
    IL_0001:  ldstr      "PlugCollectionInto"
    IL_0006:  ldstr      "http://schemas.microsoft.com/wix/VSExtension"
    IL_000b:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteStartElement(string,
                                                                                           string)
    IL_0010:  ldarg.0
    IL_0011:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::attributesFieldSet
    IL_0016:  brfalse.s  IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      "Attributes"
    IL_001e:  ldarg.0
    IL_001f:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::attributesField
    IL_0024:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0029:  ldarg.0
    IL_002a:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::tableOfContentsFieldSet
    IL_002f:  brfalse.s  IL_0042

    IL_0031:  ldarg.1
    IL_0032:  ldstr      "TableOfContents"
    IL_0037:  ldarg.0
    IL_0038:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::tableOfContentsField
    IL_003d:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0042:  ldarg.0
    IL_0043:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetCollectionFieldSet
    IL_0048:  brfalse.s  IL_005b

    IL_004a:  ldarg.1
    IL_004b:  ldstr      "TargetCollection"
    IL_0050:  ldarg.0
    IL_0051:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetCollectionField
    IL_0056:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_005b:  ldarg.0
    IL_005c:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetTableOfContentsFieldSet
    IL_0061:  brfalse.s  IL_0074

    IL_0063:  ldarg.1
    IL_0064:  ldstr      "TargetTableOfContents"
    IL_0069:  ldarg.0
    IL_006a:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetTableOfContentsField
    IL_006f:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0074:  ldarg.1
    IL_0075:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteEndElement()
    IL_007a:  ret
  } // end of method PlugCollectionInto::OutputXml

  .method private hidebysig newslot virtual final 
          instance void  Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute(string name,
                                                                                                   string 'value') cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes::SetAttribute
    // Code size       109 (0x6d)
    .maxstack  2
    IL_0000:  ldstr      "Attributes"
    IL_0005:  ldarg.1
    IL_0006:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000b:  brfalse.s  IL_001b

    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::attributesField
    IL_0014:  ldarg.0
    IL_0015:  ldc.i4.1
    IL_0016:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::attributesFieldSet
    IL_001b:  ldstr      "TableOfContents"
    IL_0020:  ldarg.1
    IL_0021:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0026:  brfalse.s  IL_0036

    IL_0028:  ldarg.0
    IL_0029:  ldarg.2
    IL_002a:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::tableOfContentsField
    IL_002f:  ldarg.0
    IL_0030:  ldc.i4.1
    IL_0031:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::tableOfContentsFieldSet
    IL_0036:  ldstr      "TargetCollection"
    IL_003b:  ldarg.1
    IL_003c:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0041:  brfalse.s  IL_0051

    IL_0043:  ldarg.0
    IL_0044:  ldarg.2
    IL_0045:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetCollectionField
    IL_004a:  ldarg.0
    IL_004b:  ldc.i4.1
    IL_004c:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetCollectionFieldSet
    IL_0051:  ldstr      "TargetTableOfContents"
    IL_0056:  ldarg.1
    IL_0057:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_005c:  brfalse.s  IL_006c

    IL_005e:  ldarg.0
    IL_005f:  ldarg.2
    IL_0060:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetTableOfContentsField
    IL_0065:  ldarg.0
    IL_0066:  ldc.i4.1
    IL_0067:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::targetTableOfContentsFieldSet
    IL_006c:  ret
  } // end of method PlugCollectionInto::Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method PlugCollectionInto::.ctor

  .property instance string Attributes()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::get_Attributes()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_Attributes(string)
  } // end of property PlugCollectionInto::Attributes
  .property instance string TableOfContents()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::get_TableOfContents()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_TableOfContents(string)
  } // end of property PlugCollectionInto::TableOfContents
  .property instance string TargetCollection()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::get_TargetCollection()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_TargetCollection(string)
  } // end of property PlugCollectionInto::TargetCollection
  .property instance string TargetTableOfContents()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::get_TargetTableOfContents()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_TargetTableOfContents(string)
  } // end of property PlugCollectionInto::TargetTableOfContents
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
          ParentElement()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::get_ParentElement()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
  } // end of property PlugCollectionInto::ParentElement
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto

.class public auto ansi beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef
       extends [mscorlib]System.Object
       implements [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes
{
  .field private string idField
  .field private bool idFieldSet
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement parentElement
  .method public hidebysig specialname instance string 
          get_Id() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::idField
    IL_0006:  ret
  } // end of method HelpFilterRef::get_Id

  .method public hidebysig specialname instance void 
          set_Id(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::idFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::idField
    IL_000e:  ret
  } // end of method HelpFilterRef::set_Id

  .method public hidebysig newslot specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          get_ParentElement() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::parentElement
    IL_0006:  ret
  } // end of method HelpFilterRef::get_ParentElement

  .method public hidebysig newslot specialname virtual 
          instance void  set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 'value') cil managed
  {
    // Code size       8 (0x8)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::parentElement
    IL_0007:  ret
  } // end of method HelpFilterRef::set_ParentElement

  .method public hidebysig newslot virtual 
          instance void  OutputXml(class [System.Xml]System.Xml.XmlTextWriter writer) cil managed
  {
    // Code size       48 (0x30)
    .maxstack  3
    IL_0000:  ldarg.1
    IL_0001:  ldstr      "HelpFilterRef"
    IL_0006:  ldstr      "http://schemas.microsoft.com/wix/VSExtension"
    IL_000b:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteStartElement(string,
                                                                                           string)
    IL_0010:  ldarg.0
    IL_0011:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::idFieldSet
    IL_0016:  brfalse.s  IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      "Id"
    IL_001e:  ldarg.0
    IL_001f:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::idField
    IL_0024:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0029:  ldarg.1
    IL_002a:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteEndElement()
    IL_002f:  ret
  } // end of method HelpFilterRef::OutputXml

  .method private hidebysig newslot virtual final 
          instance void  Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute(string name,
                                                                                                   string 'value') cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes::SetAttribute
    // Code size       28 (0x1c)
    .maxstack  2
    IL_0000:  ldstr      "Id"
    IL_0005:  ldarg.1
    IL_0006:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000b:  brfalse.s  IL_001b

    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::idField
    IL_0014:  ldarg.0
    IL_0015:  ldc.i4.1
    IL_0016:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::idFieldSet
    IL_001b:  ret
  } // end of method HelpFilterRef::Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method HelpFilterRef::.ctor

  .property instance string Id()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::get_Id()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::set_Id(string)
  } // end of property HelpFilterRef::Id
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
          ParentElement()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::get_ParentElement()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
  } // end of property HelpFilterRef::ParentElement
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef

.class public auto ansi beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection
       extends [mscorlib]System.Object
       implements [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.IParentElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes
{
  .custom instance void [mscorlib]System.Reflection.DefaultMemberAttribute::.ctor(string)
           = {string('Item')}
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection children
  .field private string idField
  .field private bool idFieldSet
  .field private string descriptionField
  .field private bool descriptionFieldSet
  .field private string nameField
  .field private bool nameFieldSet
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement parentElement
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       84 (0x54)
    .maxstack  3
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection V_0)
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ldc.i4.0
    IL_0007:  newobj     instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::.ctor(valuetype [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/CollectionType)
    IL_000c:  stloc.0
    IL_000d:  ldloc.0
    IL_000e:  ldtoken    Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef
    IL_0013:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_0018:  newobj     instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/ChoiceItem::.ctor(class [mscorlib]System.Type)
    IL_001d:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::AddItem(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/CollectionItem)
    IL_0022:  ldloc.0
    IL_0023:  ldtoken    Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef
    IL_0028:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_002d:  newobj     instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/ChoiceItem::.ctor(class [mscorlib]System.Type)
    IL_0032:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::AddItem(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/CollectionItem)
    IL_0037:  ldloc.0
    IL_0038:  ldtoken    Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto
    IL_003d:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_0042:  newobj     instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/ChoiceItem::.ctor(class [mscorlib]System.Type)
    IL_0047:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::AddItem(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/CollectionItem)
    IL_004c:  ldarg.0
    IL_004d:  ldloc.0
    IL_004e:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::children
    IL_0053:  ret
  } // end of method HelpCollection::.ctor

  .method public hidebysig newslot specialname virtual 
          instance class [mscorlib]System.Collections.IEnumerable 
          get_Children() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::children
    IL_0006:  ret
  } // end of method HelpCollection::get_Children

  .method public hidebysig newslot specialname virtual 
          instance class [mscorlib]System.Collections.IEnumerable 
          get_Item(class [mscorlib]System.Type childType) cil managed
  {
    // Code size       13 (0xd)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::children
    IL_0006:  ldarg.1
    IL_0007:  callvirt   instance class [mscorlib]System.Collections.IEnumerable [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::Filter(class [mscorlib]System.Type)
    IL_000c:  ret
  } // end of method HelpCollection::get_Item

  .method public hidebysig specialname instance string 
          get_Id() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::idField
    IL_0006:  ret
  } // end of method HelpCollection::get_Id

  .method public hidebysig specialname instance void 
          set_Id(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::idFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::idField
    IL_000e:  ret
  } // end of method HelpCollection::set_Id

  .method public hidebysig specialname instance string 
          get_Description() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::descriptionField
    IL_0006:  ret
  } // end of method HelpCollection::get_Description

  .method public hidebysig specialname instance void 
          set_Description(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::descriptionFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::descriptionField
    IL_000e:  ret
  } // end of method HelpCollection::set_Description

  .method public hidebysig specialname instance string 
          get_Name() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::nameField
    IL_0006:  ret
  } // end of method HelpCollection::get_Name

  .method public hidebysig specialname instance void 
          set_Name(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::nameFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::nameField
    IL_000e:  ret
  } // end of method HelpCollection::set_Name

  .method public hidebysig newslot specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          get_ParentElement() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::parentElement
    IL_0006:  ret
  } // end of method HelpCollection::get_ParentElement

  .method public hidebysig newslot specialname virtual 
          instance void  set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 'value') cil managed
  {
    // Code size       8 (0x8)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::parentElement
    IL_0007:  ret
  } // end of method HelpCollection::set_ParentElement

  .method public hidebysig newslot virtual 
          instance void  AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement child) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::children
    IL_0006:  ldarg.1
    IL_0007:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::AddElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_000c:  ldarg.1
    IL_000d:  ldarg.0
    IL_000e:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_0013:  ret
  } // end of method HelpCollection::AddChild

  .method public hidebysig newslot virtual 
          instance void  RemoveChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement child) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::children
    IL_0006:  ldarg.1
    IL_0007:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::RemoveElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_000c:  ldarg.1
    IL_000d:  ldnull
    IL_000e:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_0013:  ret
  } // end of method HelpCollection::RemoveChild

  .method private hidebysig newslot virtual final 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren.CreateChild(string childName) cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren::CreateChild
    // Code size       81 (0x51)
    .maxstack  2
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement V_0)
    IL_0000:  ldnull
    IL_0001:  stloc.0
    IL_0002:  ldstr      "HelpFileRef"
    IL_0007:  ldarg.1
    IL_0008:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000d:  brfalse.s  IL_0015

    IL_000f:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::.ctor()
    IL_0014:  stloc.0
    IL_0015:  ldstr      "HelpFilterRef"
    IL_001a:  ldarg.1
    IL_001b:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0020:  brfalse.s  IL_0028

    IL_0022:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFilterRef::.ctor()
    IL_0027:  stloc.0
    IL_0028:  ldstr      "PlugCollectionInto"
    IL_002d:  ldarg.1
    IL_002e:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0033:  brfalse.s  IL_003b

    IL_0035:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.PlugCollectionInto::.ctor()
    IL_003a:  stloc.0
    IL_003b:  ldloc.0
    IL_003c:  brtrue.s   IL_004f

    IL_003e:  ldarg.1
    IL_003f:  ldstr      " is not a valid child name."
    IL_0044:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_0049:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
    IL_004e:  throw

    IL_004f:  ldloc.0
    IL_0050:  ret
  } // end of method HelpCollection::Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren.CreateChild

  .method public hidebysig newslot virtual 
          instance void  OutputXml(class [System.Xml]System.Xml.XmlTextWriter writer) cil managed
  {
    // Code size       139 (0x8b)
    .maxstack  3
    .locals init (class [mscorlib]System.Collections.IEnumerator V_0,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement V_1)
    IL_0000:  ldarg.1
    IL_0001:  ldstr      "HelpCollection"
    IL_0006:  ldstr      "http://schemas.microsoft.com/wix/VSExtension"
    IL_000b:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteStartElement(string,
                                                                                           string)
    IL_0010:  ldarg.0
    IL_0011:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::idFieldSet
    IL_0016:  brfalse.s  IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      "Id"
    IL_001e:  ldarg.0
    IL_001f:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::idField
    IL_0024:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0029:  ldarg.0
    IL_002a:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::descriptionFieldSet
    IL_002f:  brfalse.s  IL_0042

    IL_0031:  ldarg.1
    IL_0032:  ldstr      "Description"
    IL_0037:  ldarg.0
    IL_0038:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::descriptionField
    IL_003d:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0042:  ldarg.0
    IL_0043:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::nameFieldSet
    IL_0048:  brfalse.s  IL_005b

    IL_004a:  ldarg.1
    IL_004b:  ldstr      "Name"
    IL_0050:  ldarg.0
    IL_0051:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::nameField
    IL_0056:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_005b:  ldarg.0
    IL_005c:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::children
    IL_0061:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::GetEnumerator()
    IL_0066:  stloc.0
    IL_0067:  br.s       IL_007c

    IL_0069:  ldloc.0
    IL_006a:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
    IL_006f:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
    IL_0074:  stloc.1
    IL_0075:  ldloc.1
    IL_0076:  ldarg.1
    IL_0077:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement::OutputXml(class [System.Xml]System.Xml.XmlTextWriter)
    IL_007c:  ldloc.0
    IL_007d:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
    IL_0082:  brtrue.s   IL_0069

    IL_0084:  ldarg.1
    IL_0085:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteEndElement()
    IL_008a:  ret
  } // end of method HelpCollection::OutputXml

  .method private hidebysig newslot virtual final 
          instance void  Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute(string name,
                                                                                                   string 'value') cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes::SetAttribute
    // Code size       82 (0x52)
    .maxstack  2
    IL_0000:  ldstr      "Id"
    IL_0005:  ldarg.1
    IL_0006:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000b:  brfalse.s  IL_001b

    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::idField
    IL_0014:  ldarg.0
    IL_0015:  ldc.i4.1
    IL_0016:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::idFieldSet
    IL_001b:  ldstr      "Description"
    IL_0020:  ldarg.1
    IL_0021:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0026:  brfalse.s  IL_0036

    IL_0028:  ldarg.0
    IL_0029:  ldarg.2
    IL_002a:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::descriptionField
    IL_002f:  ldarg.0
    IL_0030:  ldc.i4.1
    IL_0031:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::descriptionFieldSet
    IL_0036:  ldstr      "Name"
    IL_003b:  ldarg.1
    IL_003c:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0041:  brfalse.s  IL_0051

    IL_0043:  ldarg.0
    IL_0044:  ldarg.2
    IL_0045:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::nameField
    IL_004a:  ldarg.0
    IL_004b:  ldc.i4.1
    IL_004c:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::nameFieldSet
    IL_0051:  ret
  } // end of method HelpCollection::Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute

  .property instance class [mscorlib]System.Collections.IEnumerable
          Children()
  {
    .get instance class [mscorlib]System.Collections.IEnumerable Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::get_Children()
  } // end of property HelpCollection::Children
  .property instance class [mscorlib]System.Collections.IEnumerable
          Item(class [mscorlib]System.Type)
  {
    .get instance class [mscorlib]System.Collections.IEnumerable Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::get_Item(class [mscorlib]System.Type)
  } // end of property HelpCollection::Item
  .property instance string Id()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::get_Id()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::set_Id(string)
  } // end of property HelpCollection::Id
  .property instance string Description()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::get_Description()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::set_Description(string)
  } // end of property HelpCollection::Description
  .property instance string Name()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::get_Name()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::set_Name(string)
  } // end of property HelpCollection::Name
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
          ParentElement()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::get_ParentElement()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
  } // end of property HelpCollection::ParentElement
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollection

.class public auto ansi beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile
       extends [mscorlib]System.Object
       implements [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes
{
  .field private string idField
  .field private bool idFieldSet
  .field private string attributeIndexField
  .field private bool attributeIndexFieldSet
  .field private string indexField
  .field private bool indexFieldSet
  .field private int32 languageField
  .field private bool languageFieldSet
  .field private string nameField
  .field private bool nameFieldSet
  .field private string sampleLocationField
  .field private bool sampleLocationFieldSet
  .field private string searchField
  .field private bool searchFieldSet
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement parentElement
  .method public hidebysig specialname instance string 
          get_Id() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::idField
    IL_0006:  ret
  } // end of method HelpFile::get_Id

  .method public hidebysig specialname instance void 
          set_Id(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::idFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::idField
    IL_000e:  ret
  } // end of method HelpFile::set_Id

  .method public hidebysig specialname instance string 
          get_AttributeIndex() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::attributeIndexField
    IL_0006:  ret
  } // end of method HelpFile::get_AttributeIndex

  .method public hidebysig specialname instance void 
          set_AttributeIndex(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::attributeIndexFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::attributeIndexField
    IL_000e:  ret
  } // end of method HelpFile::set_AttributeIndex

  .method public hidebysig specialname instance string 
          get_Index() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::indexField
    IL_0006:  ret
  } // end of method HelpFile::get_Index

  .method public hidebysig specialname instance void 
          set_Index(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::indexFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::indexField
    IL_000e:  ret
  } // end of method HelpFile::set_Index

  .method public hidebysig specialname instance int32 
          get_Language() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      int32 Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::languageField
    IL_0006:  ret
  } // end of method HelpFile::get_Language

  .method public hidebysig specialname instance void 
          set_Language(int32 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::languageFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      int32 Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::languageField
    IL_000e:  ret
  } // end of method HelpFile::set_Language

  .method public hidebysig specialname instance string 
          get_Name() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::nameField
    IL_0006:  ret
  } // end of method HelpFile::get_Name

  .method public hidebysig specialname instance void 
          set_Name(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::nameFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::nameField
    IL_000e:  ret
  } // end of method HelpFile::set_Name

  .method public hidebysig specialname instance string 
          get_SampleLocation() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::sampleLocationField
    IL_0006:  ret
  } // end of method HelpFile::get_SampleLocation

  .method public hidebysig specialname instance void 
          set_SampleLocation(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::sampleLocationFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::sampleLocationField
    IL_000e:  ret
  } // end of method HelpFile::set_SampleLocation

  .method public hidebysig specialname instance string 
          get_Search() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::searchField
    IL_0006:  ret
  } // end of method HelpFile::get_Search

  .method public hidebysig specialname instance void 
          set_Search(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::searchFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::searchField
    IL_000e:  ret
  } // end of method HelpFile::set_Search

  .method public hidebysig newslot specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          get_ParentElement() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::parentElement
    IL_0006:  ret
  } // end of method HelpFile::get_ParentElement

  .method public hidebysig newslot specialname virtual 
          instance void  set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 'value') cil managed
  {
    // Code size       8 (0x8)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::parentElement
    IL_0007:  ret
  } // end of method HelpFile::set_ParentElement

  .method public hidebysig newslot virtual 
          instance void  OutputXml(class [System.Xml]System.Xml.XmlTextWriter writer) cil managed
  {
    // Code size       208 (0xd0)
    .maxstack  4
    IL_0000:  ldarg.1
    IL_0001:  ldstr      "HelpFile"
    IL_0006:  ldstr      "http://schemas.microsoft.com/wix/VSExtension"
    IL_000b:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteStartElement(string,
                                                                                           string)
    IL_0010:  ldarg.0
    IL_0011:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::idFieldSet
    IL_0016:  brfalse.s  IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      "Id"
    IL_001e:  ldarg.0
    IL_001f:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::idField
    IL_0024:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0029:  ldarg.0
    IL_002a:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::attributeIndexFieldSet
    IL_002f:  brfalse.s  IL_0042

    IL_0031:  ldarg.1
    IL_0032:  ldstr      "AttributeIndex"
    IL_0037:  ldarg.0
    IL_0038:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::attributeIndexField
    IL_003d:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0042:  ldarg.0
    IL_0043:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::indexFieldSet
    IL_0048:  brfalse.s  IL_005b

    IL_004a:  ldarg.1
    IL_004b:  ldstr      "Index"
    IL_0050:  ldarg.0
    IL_0051:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::indexField
    IL_0056:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_005b:  ldarg.0
    IL_005c:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::languageFieldSet
    IL_0061:  brfalse.s  IL_007e

    IL_0063:  ldarg.1
    IL_0064:  ldstr      "Language"
    IL_0069:  ldarg.0
    IL_006a:  ldflda     int32 Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::languageField
    IL_006f:  call       class [mscorlib]System.Globalization.CultureInfo [mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()
    IL_0074:  call       instance string [mscorlib]System.Int32::ToString(class [mscorlib]System.IFormatProvider)
    IL_0079:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_007e:  ldarg.0
    IL_007f:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::nameFieldSet
    IL_0084:  brfalse.s  IL_0097

    IL_0086:  ldarg.1
    IL_0087:  ldstr      "Name"
    IL_008c:  ldarg.0
    IL_008d:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::nameField
    IL_0092:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0097:  ldarg.0
    IL_0098:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::sampleLocationFieldSet
    IL_009d:  brfalse.s  IL_00b0

    IL_009f:  ldarg.1
    IL_00a0:  ldstr      "SampleLocation"
    IL_00a5:  ldarg.0
    IL_00a6:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::sampleLocationField
    IL_00ab:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_00b0:  ldarg.0
    IL_00b1:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::searchFieldSet
    IL_00b6:  brfalse.s  IL_00c9

    IL_00b8:  ldarg.1
    IL_00b9:  ldstr      "Search"
    IL_00be:  ldarg.0
    IL_00bf:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::searchField
    IL_00c4:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_00c9:  ldarg.1
    IL_00ca:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteEndElement()
    IL_00cf:  ret
  } // end of method HelpFile::OutputXml

  .method private hidebysig newslot virtual final 
          instance void  Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute(string name,
                                                                                                   string 'value') cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes::SetAttribute
    // Code size       200 (0xc8)
    .maxstack  3
    IL_0000:  ldstr      "Id"
    IL_0005:  ldarg.1
    IL_0006:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000b:  brfalse.s  IL_001b

    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::idField
    IL_0014:  ldarg.0
    IL_0015:  ldc.i4.1
    IL_0016:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::idFieldSet
    IL_001b:  ldstr      "AttributeIndex"
    IL_0020:  ldarg.1
    IL_0021:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0026:  brfalse.s  IL_0036

    IL_0028:  ldarg.0
    IL_0029:  ldarg.2
    IL_002a:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::attributeIndexField
    IL_002f:  ldarg.0
    IL_0030:  ldc.i4.1
    IL_0031:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::attributeIndexFieldSet
    IL_0036:  ldstr      "Index"
    IL_003b:  ldarg.1
    IL_003c:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0041:  brfalse.s  IL_0051

    IL_0043:  ldarg.0
    IL_0044:  ldarg.2
    IL_0045:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::indexField
    IL_004a:  ldarg.0
    IL_004b:  ldc.i4.1
    IL_004c:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::indexFieldSet
    IL_0051:  ldstr      "Language"
    IL_0056:  ldarg.1
    IL_0057:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_005c:  brfalse.s  IL_0076

    IL_005e:  ldarg.0
    IL_005f:  ldarg.2
    IL_0060:  call       class [mscorlib]System.Globalization.CultureInfo [mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()
    IL_0065:  call       int32 [mscorlib]System.Convert::ToInt32(string,
                                                                 class [mscorlib]System.IFormatProvider)
    IL_006a:  stfld      int32 Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::languageField
    IL_006f:  ldarg.0
    IL_0070:  ldc.i4.1
    IL_0071:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::languageFieldSet
    IL_0076:  ldstr      "Name"
    IL_007b:  ldarg.1
    IL_007c:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0081:  brfalse.s  IL_0091

    IL_0083:  ldarg.0
    IL_0084:  ldarg.2
    IL_0085:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::nameField
    IL_008a:  ldarg.0
    IL_008b:  ldc.i4.1
    IL_008c:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::nameFieldSet
    IL_0091:  ldstr      "SampleLocation"
    IL_0096:  ldarg.1
    IL_0097:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_009c:  brfalse.s  IL_00ac

    IL_009e:  ldarg.0
    IL_009f:  ldarg.2
    IL_00a0:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::sampleLocationField
    IL_00a5:  ldarg.0
    IL_00a6:  ldc.i4.1
    IL_00a7:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::sampleLocationFieldSet
    IL_00ac:  ldstr      "Search"
    IL_00b1:  ldarg.1
    IL_00b2:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_00b7:  brfalse.s  IL_00c7

    IL_00b9:  ldarg.0
    IL_00ba:  ldarg.2
    IL_00bb:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::searchField
    IL_00c0:  ldarg.0
    IL_00c1:  ldc.i4.1
    IL_00c2:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::searchFieldSet
    IL_00c7:  ret
  } // end of method HelpFile::Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method HelpFile::.ctor

  .property instance string Id()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_Id()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Id(string)
  } // end of property HelpFile::Id
  .property instance string AttributeIndex()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_AttributeIndex()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_AttributeIndex(string)
  } // end of property HelpFile::AttributeIndex
  .property instance string Index()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_Index()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Index(string)
  } // end of property HelpFile::Index
  .property instance int32 Language()
  {
    .get instance int32 Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_Language()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Language(int32)
  } // end of property HelpFile::Language
  .property instance string Name()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_Name()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Name(string)
  } // end of property HelpFile::Name
  .property instance string SampleLocation()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_SampleLocation()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_SampleLocation(string)
  } // end of property HelpFile::SampleLocation
  .property instance string Search()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_Search()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_Search(string)
  } // end of property HelpFile::Search
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
          ParentElement()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::get_ParentElement()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
  } // end of property HelpFile::ParentElement
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFile

.class public auto ansi beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef
       extends [mscorlib]System.Object
       implements [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.IParentElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes
{
  .custom instance void [mscorlib]System.Reflection.DefaultMemberAttribute::.ctor(string)
           = {string('Item')}
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection children
  .field private string idField
  .field private bool idFieldSet
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement parentElement
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       42 (0x2a)
    .maxstack  3
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection V_0)
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ldc.i4.0
    IL_0007:  newobj     instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::.ctor(valuetype [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/CollectionType)
    IL_000c:  stloc.0
    IL_000d:  ldloc.0
    IL_000e:  ldtoken    Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef
    IL_0013:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_0018:  newobj     instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/ChoiceItem::.ctor(class [mscorlib]System.Type)
    IL_001d:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::AddItem(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection/CollectionItem)
    IL_0022:  ldarg.0
    IL_0023:  ldloc.0
    IL_0024:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::children
    IL_0029:  ret
  } // end of method HelpCollectionRef::.ctor

  .method public hidebysig newslot specialname virtual 
          instance class [mscorlib]System.Collections.IEnumerable 
          get_Children() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::children
    IL_0006:  ret
  } // end of method HelpCollectionRef::get_Children

  .method public hidebysig newslot specialname virtual 
          instance class [mscorlib]System.Collections.IEnumerable 
          get_Item(class [mscorlib]System.Type childType) cil managed
  {
    // Code size       13 (0xd)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::children
    IL_0006:  ldarg.1
    IL_0007:  callvirt   instance class [mscorlib]System.Collections.IEnumerable [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::Filter(class [mscorlib]System.Type)
    IL_000c:  ret
  } // end of method HelpCollectionRef::get_Item

  .method public hidebysig specialname instance string 
          get_Id() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::idField
    IL_0006:  ret
  } // end of method HelpCollectionRef::get_Id

  .method public hidebysig specialname instance void 
          set_Id(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::idFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::idField
    IL_000e:  ret
  } // end of method HelpCollectionRef::set_Id

  .method public hidebysig newslot specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          get_ParentElement() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::parentElement
    IL_0006:  ret
  } // end of method HelpCollectionRef::get_ParentElement

  .method public hidebysig newslot specialname virtual 
          instance void  set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 'value') cil managed
  {
    // Code size       8 (0x8)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::parentElement
    IL_0007:  ret
  } // end of method HelpCollectionRef::set_ParentElement

  .method public hidebysig newslot virtual 
          instance void  AddChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement child) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::children
    IL_0006:  ldarg.1
    IL_0007:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::AddElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_000c:  ldarg.1
    IL_000d:  ldarg.0
    IL_000e:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_0013:  ret
  } // end of method HelpCollectionRef::AddChild

  .method public hidebysig newslot virtual 
          instance void  RemoveChild(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement child) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::children
    IL_0006:  ldarg.1
    IL_0007:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::RemoveElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_000c:  ldarg.1
    IL_000d:  ldnull
    IL_000e:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
    IL_0013:  ret
  } // end of method HelpCollectionRef::RemoveChild

  .method private hidebysig newslot virtual final 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren.CreateChild(string childName) cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren::CreateChild
    // Code size       43 (0x2b)
    .maxstack  2
    .locals init (class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement V_0)
    IL_0000:  ldnull
    IL_0001:  stloc.0
    IL_0002:  ldstr      "HelpFileRef"
    IL_0007:  ldarg.1
    IL_0008:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000d:  brfalse.s  IL_0015

    IL_000f:  newobj     instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::.ctor()
    IL_0014:  stloc.0
    IL_0015:  ldloc.0
    IL_0016:  brtrue.s   IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      " is not a valid child name."
    IL_001e:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_0023:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
    IL_0028:  throw

    IL_0029:  ldloc.0
    IL_002a:  ret
  } // end of method HelpCollectionRef::Microsoft.Tools.WindowsInstallerXml.Serialize.ICreateChildren.CreateChild

  .method public hidebysig newslot virtual 
          instance void  OutputXml(class [System.Xml]System.Xml.XmlTextWriter writer) cil managed
  {
    // Code size       89 (0x59)
    .maxstack  3
    .locals init (class [mscorlib]System.Collections.IEnumerator V_0,
             class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement V_1)
    IL_0000:  ldarg.1
    IL_0001:  ldstr      "HelpCollectionRef"
    IL_0006:  ldstr      "http://schemas.microsoft.com/wix/VSExtension"
    IL_000b:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteStartElement(string,
                                                                                           string)
    IL_0010:  ldarg.0
    IL_0011:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::idFieldSet
    IL_0016:  brfalse.s  IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      "Id"
    IL_001e:  ldarg.0
    IL_001f:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::idField
    IL_0024:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0029:  ldarg.0
    IL_002a:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::children
    IL_002f:  callvirt   instance class [mscorlib]System.Collections.IEnumerator [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ElementCollection::GetEnumerator()
    IL_0034:  stloc.0
    IL_0035:  br.s       IL_004a

    IL_0037:  ldloc.0
    IL_0038:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
    IL_003d:  castclass  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
    IL_0042:  stloc.1
    IL_0043:  ldloc.1
    IL_0044:  ldarg.1
    IL_0045:  callvirt   instance void [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement::OutputXml(class [System.Xml]System.Xml.XmlTextWriter)
    IL_004a:  ldloc.0
    IL_004b:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
    IL_0050:  brtrue.s   IL_0037

    IL_0052:  ldarg.1
    IL_0053:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteEndElement()
    IL_0058:  ret
  } // end of method HelpCollectionRef::OutputXml

  .method private hidebysig newslot virtual final 
          instance void  Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute(string name,
                                                                                                   string 'value') cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes::SetAttribute
    // Code size       28 (0x1c)
    .maxstack  2
    IL_0000:  ldstr      "Id"
    IL_0005:  ldarg.1
    IL_0006:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000b:  brfalse.s  IL_001b

    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::idField
    IL_0014:  ldarg.0
    IL_0015:  ldc.i4.1
    IL_0016:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::idFieldSet
    IL_001b:  ret
  } // end of method HelpCollectionRef::Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute

  .property instance class [mscorlib]System.Collections.IEnumerable
          Children()
  {
    .get instance class [mscorlib]System.Collections.IEnumerable Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::get_Children()
  } // end of property HelpCollectionRef::Children
  .property instance class [mscorlib]System.Collections.IEnumerable
          Item(class [mscorlib]System.Type)
  {
    .get instance class [mscorlib]System.Collections.IEnumerable Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::get_Item(class [mscorlib]System.Type)
  } // end of property HelpCollectionRef::Item
  .property instance string Id()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::get_Id()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::set_Id(string)
  } // end of property HelpCollectionRef::Id
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
          ParentElement()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::get_ParentElement()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
  } // end of property HelpCollectionRef::ParentElement
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpCollectionRef

.class public auto ansi beforefieldinit Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef
       extends [mscorlib]System.Object
       implements [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement,
                  [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes
{
  .field private string idField
  .field private bool idFieldSet
  .field private class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement parentElement
  .method public hidebysig specialname instance string 
          get_Id() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::idField
    IL_0006:  ret
  } // end of method HelpFileRef::get_Id

  .method public hidebysig specialname instance void 
          set_Id(string 'value') cil managed
  {
    // Code size       15 (0xf)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::idFieldSet
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::idField
    IL_000e:  ret
  } // end of method HelpFileRef::set_Id

  .method public hidebysig newslot specialname virtual 
          instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 
          get_ParentElement() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::parentElement
    IL_0006:  ret
  } // end of method HelpFileRef::get_ParentElement

  .method public hidebysig newslot specialname virtual 
          instance void  set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement 'value') cil managed
  {
    // Code size       8 (0x8)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::parentElement
    IL_0007:  ret
  } // end of method HelpFileRef::set_ParentElement

  .method public hidebysig newslot virtual 
          instance void  OutputXml(class [System.Xml]System.Xml.XmlTextWriter writer) cil managed
  {
    // Code size       48 (0x30)
    .maxstack  3
    IL_0000:  ldarg.1
    IL_0001:  ldstr      "HelpFileRef"
    IL_0006:  ldstr      "http://schemas.microsoft.com/wix/VSExtension"
    IL_000b:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteStartElement(string,
                                                                                           string)
    IL_0010:  ldarg.0
    IL_0011:  ldfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::idFieldSet
    IL_0016:  brfalse.s  IL_0029

    IL_0018:  ldarg.1
    IL_0019:  ldstr      "Id"
    IL_001e:  ldarg.0
    IL_001f:  ldfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::idField
    IL_0024:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteAttributeString(string,
                                                                                              string)
    IL_0029:  ldarg.1
    IL_002a:  callvirt   instance void [System.Xml]System.Xml.XmlWriter::WriteEndElement()
    IL_002f:  ret
  } // end of method HelpFileRef::OutputXml

  .method private hidebysig newslot virtual final 
          instance void  Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute(string name,
                                                                                                   string 'value') cil managed
  {
    .override [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes::SetAttribute
    // Code size       28 (0x1c)
    .maxstack  2
    IL_0000:  ldstr      "Id"
    IL_0005:  ldarg.1
    IL_0006:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_000b:  brfalse.s  IL_001b

    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  stfld      string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::idField
    IL_0014:  ldarg.0
    IL_0015:  ldc.i4.1
    IL_0016:  stfld      bool Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::idFieldSet
    IL_001b:  ret
  } // end of method HelpFileRef::Microsoft.Tools.WindowsInstallerXml.Serialize.ISetAttributes.SetAttribute

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method HelpFileRef::.ctor

  .property instance string Id()
  {
    .get instance string Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::get_Id()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::set_Id(string)
  } // end of property HelpFileRef::Id
  .property instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement
          ParentElement()
  {
    .get instance class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::get_ParentElement()
    .set instance void Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef::set_ParentElement(class [wix]Microsoft.Tools.WindowsInstallerXml.Serialize.ISchemaElement)
  } // end of property HelpFileRef::ParentElement
} // end of class Microsoft.Tools.WindowsInstallerXml.Extensions.Serialize.VS.HelpFileRef


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file WixVSExtension.res
