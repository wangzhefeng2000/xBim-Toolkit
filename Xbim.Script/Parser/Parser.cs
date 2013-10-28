// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.0
// Machine:  CENTAURUS
// DateTime: 3.10.2013 20:01:08
// UserName: Martin
// Input file <Parser.y - 25.9.2013 23:07:08>

// options: conflicts lines gplex conflicts listing

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Xbim.Gppg;
using Xbim.XbimExtensions.Interfaces;
using System.Linq.Expressions;

namespace Xbim.Script
{
internal enum Tokens {error=60,
    EOF=61,INTEGER=62,DOUBLE=63,STRING=64,BOOLEAN=65,NONDEF=66,
    DEFINED=67,IDENTIFIER=68,OP_EQ=69,OP_NEQ=70,OP_GT=71,OP_LT=72,
    OP_GTE=73,OP_LTQ=74,OP_CONTAINS=75,OP_NOT_CONTAINS=76,OP_AND=77,OP_OR=78,
    PRODUCT=79,PRODUCT_TYPE=80,FILE=81,MODEL=82,CLASSIFICATION=83,PROPERTY_SET=84,
    WHERE=85,WITH_NAME=86,DESCRIPTION=87,NEW=88,ADD=89,TO=90,
    REMOVE=91,FROM=92,FOR=93,NAME=94,PREDEFINED_TYPE=95,TYPE=96,
    MATERIAL=97,THICKNESS=98,GROUP=99,IN=100,IT=101,SELECT=102,
    SET=103,CREATE=104,DUMP=105,CLEAR=106,OPEN=107,CLOSE=108,
    SAVE=109,NORTH_OF=110,SOUTH_OF=111,WEST_OF=112,EAST_OF=113,ABOVE=114,
    BELOW=115,SPATIALLY_EQUALS=116,DISJOINT=117,INTERSECTS=118,TOUCHES=119,CROSSES=120,
    WITHIN=121,SPATIALLY_CONTAINS=122,OVERLAPS=123,RELATE=124};

internal partial struct ValueType
#line 15 "Parser.y"
{
#line 16 "Parser.y"
		public string strVal;
#line 17 "Parser.y"
		public int intVal;
#line 18 "Parser.y"
		public double doubleVal;
#line 19 "Parser.y"
		public bool boolVal;
#line 20 "Parser.y"
		public Type typeVal;
#line 21 "Parser.y"
		public object val;
#line 22 "Parser.y"
	  }
// Abstract base class for GPLEX scanners
internal abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
internal class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

internal partial class Parser: ShiftReduceParser<ValueType, LexLocation>
{
  // Verbatim content from Parser.y - 25.9.2013 23:07:08
#line 2 "Parser.y"
	
  // End verbatim content from Parser.y - 25.9.2013 23:07:08

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[116];
  private static State[] states = new State[191];
  private static string[] nonTerms = new string[] {
      "expressions", "$accept", "expression", "selection", "creation", "addition", 
      "attr_setting", "variables_actions", "model_actions", "value_setting_list", 
      "value_setting", "attribute", "value", "num_value", "string_list", "selection_statement", 
      "op_bool", "object", "conditions", "creation_statement", "condition", "attributeCondition", 
      "materialCondition", "typeCondition", "propertyCondition", "groupCondition", 
      "spatialCondition", "op_cont", "op_num_rel", "op_spatial", };

  static Parser() {
    states[0] = new State(new int[]{102,6,68,106,104,121,89,127,91,131,103,137,105,160,106,173,107,177,108,182,109,184,60,189},new int[]{-1,1,-3,190,-4,4,-5,119,-6,125,-7,135,-8,158,-9,175});
    states[1] = new State(new int[]{61,2,102,6,68,106,104,121,89,127,91,131,103,137,105,160,106,173,107,177,108,182,109,184,60,189},new int[]{-3,3,-4,4,-5,119,-6,125,-7,135,-8,158,-9,175});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{59,5});
    states[5] = new State(-4);
    states[6] = new State(new int[]{79,102,80,103,97,104,99,105},new int[]{-16,7,-18,8});
    states[7] = new State(-35);
    states[8] = new State(new int[]{64,9,85,10,59,-37});
    states[9] = new State(-38);
    states[10] = new State(new int[]{94,25,87,26,95,27,97,29,98,34,96,46,64,59,99,77,101,81,77,-51,78,-51,59,-51},new int[]{-19,11,-21,101,-22,14,-12,15,-23,28,-24,45,-25,75,-26,76,-27,80});
    states[11] = new State(new int[]{77,12,78,99,59,-39});
    states[12] = new State(new int[]{94,25,87,26,95,27,97,29,98,34,96,46,64,59,99,77,101,81,77,-51,78,-51,59,-51},new int[]{-21,13,-22,14,-12,15,-23,28,-24,45,-25,75,-26,76,-27,80});
    states[13] = new State(-48);
    states[14] = new State(-52);
    states[15] = new State(new int[]{69,21,70,22,75,23,76,24},new int[]{-17,16,-28,19});
    states[16] = new State(new int[]{64,17,66,18});
    states[17] = new State(-58);
    states[18] = new State(-59);
    states[19] = new State(new int[]{64,20});
    states[20] = new State(-60);
    states[21] = new State(-104);
    states[22] = new State(-105);
    states[23] = new State(-110);
    states[24] = new State(-111);
    states[25] = new State(-61);
    states[26] = new State(-62);
    states[27] = new State(-63);
    states[28] = new State(-53);
    states[29] = new State(new int[]{69,21,70,22,75,23,76,24},new int[]{-17,30,-28,32});
    states[30] = new State(new int[]{64,31});
    states[31] = new State(-64);
    states[32] = new State(new int[]{64,33});
    states[33] = new State(-65);
    states[34] = new State(new int[]{69,21,70,22,71,41,72,42,73,43,74,44},new int[]{-17,35,-29,39});
    states[35] = new State(new int[]{63,37,62,38},new int[]{-14,36});
    states[36] = new State(-66);
    states[37] = new State(-22);
    states[38] = new State(-23);
    states[39] = new State(new int[]{63,37,62,38},new int[]{-14,40});
    states[40] = new State(-67);
    states[41] = new State(-106);
    states[42] = new State(-107);
    states[43] = new State(-108);
    states[44] = new State(-109);
    states[45] = new State(-54);
    states[46] = new State(new int[]{70,53,69,55,75,23,76,24,64,59,94,25,87,26,95,27},new int[]{-17,47,-28,51,-25,57,-22,58,-12,15});
    states[47] = new State(new int[]{80,48,64,49,66,50});
    states[48] = new State(-68);
    states[49] = new State(-69);
    states[50] = new State(-71);
    states[51] = new State(new int[]{64,52});
    states[52] = new State(-70);
    states[53] = new State(new int[]{67,54,80,-105,64,-105,66,-105});
    states[54] = new State(-72);
    states[55] = new State(new int[]{67,56,80,-104,64,-104,66,-104});
    states[56] = new State(-73);
    states[57] = new State(-74);
    states[58] = new State(-75);
    states[59] = new State(new int[]{70,71,69,73,71,41,72,42,73,43,74,44,75,23,76,24},new int[]{-17,60,-29,66,-28,69});
    states[60] = new State(new int[]{62,61,63,62,64,63,65,64,66,65});
    states[61] = new State(-78);
    states[62] = new State(-80);
    states[63] = new State(-82);
    states[64] = new State(-84);
    states[65] = new State(-85);
    states[66] = new State(new int[]{62,67,63,68});
    states[67] = new State(-79);
    states[68] = new State(-81);
    states[69] = new State(new int[]{64,70});
    states[70] = new State(-83);
    states[71] = new State(new int[]{67,72,62,-105,63,-105,64,-105,65,-105,66,-105});
    states[72] = new State(-86);
    states[73] = new State(new int[]{67,74,62,-104,63,-104,64,-104,65,-104,66,-104});
    states[74] = new State(-87);
    states[75] = new State(-55);
    states[76] = new State(-56);
    states[77] = new State(new int[]{64,59,94,25,87,26,95,27},new int[]{-25,78,-22,79,-12,15});
    states[78] = new State(-76);
    states[79] = new State(-77);
    states[80] = new State(-57);
    states[81] = new State(new int[]{110,84,111,85,112,86,113,87,114,88,115,89,116,90,117,91,118,92,119,93,120,94,121,95,75,96,123,97,124,98},new int[]{-30,82});
    states[82] = new State(new int[]{68,83});
    states[83] = new State(-88);
    states[84] = new State(-89);
    states[85] = new State(-90);
    states[86] = new State(-91);
    states[87] = new State(-92);
    states[88] = new State(-93);
    states[89] = new State(-94);
    states[90] = new State(-95);
    states[91] = new State(-96);
    states[92] = new State(-97);
    states[93] = new State(-98);
    states[94] = new State(-99);
    states[95] = new State(-100);
    states[96] = new State(-101);
    states[97] = new State(-102);
    states[98] = new State(-103);
    states[99] = new State(new int[]{94,25,87,26,95,27,97,29,98,34,96,46,64,59,99,77,101,81,77,-51,78,-51,59,-51},new int[]{-21,100,-22,14,-12,15,-23,28,-24,45,-25,75,-26,76,-27,80});
    states[100] = new State(-49);
    states[101] = new State(-50);
    states[102] = new State(-112);
    states[103] = new State(-113);
    states[104] = new State(-114);
    states[105] = new State(-115);
    states[106] = new State(new int[]{69,109,70,22},new int[]{-17,107});
    states[107] = new State(new int[]{79,102,80,103,97,104,99,105},new int[]{-16,108,-18,8});
    states[108] = new State(-36);
    states[109] = new State(new int[]{88,111,79,-104,80,-104,97,-104,99,-104},new int[]{-20,110});
    states[110] = new State(-42);
    states[111] = new State(new int[]{79,102,80,103,97,104,99,105},new int[]{-18,112});
    states[112] = new State(new int[]{64,113,86,114});
    states[113] = new State(-43);
    states[114] = new State(new int[]{64,115});
    states[115] = new State(new int[]{77,116,59,-44});
    states[116] = new State(new int[]{87,117});
    states[117] = new State(new int[]{64,118});
    states[118] = new State(-45);
    states[119] = new State(new int[]{59,120});
    states[120] = new State(-5);
    states[121] = new State(new int[]{83,123,88,111},new int[]{-20,122});
    states[122] = new State(-40);
    states[123] = new State(new int[]{64,124});
    states[124] = new State(-41);
    states[125] = new State(new int[]{59,126});
    states[126] = new State(-6);
    states[127] = new State(new int[]{68,128});
    states[128] = new State(new int[]{90,129});
    states[129] = new State(new int[]{68,130});
    states[130] = new State(-46);
    states[131] = new State(new int[]{68,132});
    states[132] = new State(new int[]{92,133});
    states[133] = new State(new int[]{68,134});
    states[134] = new State(-47);
    states[135] = new State(new int[]{59,136});
    states[136] = new State(-7);
    states[137] = new State(new int[]{94,25,87,26,95,27,64,154},new int[]{-10,138,-11,157,-12,146});
    states[138] = new State(new int[]{93,139,44,144});
    states[139] = new State(new int[]{68,140});
    states[140] = new State(new int[]{100,141,59,-11});
    states[141] = new State(new int[]{84,142});
    states[142] = new State(new int[]{64,143});
    states[143] = new State(-12);
    states[144] = new State(new int[]{94,25,87,26,95,27,64,154},new int[]{-11,145,-12,146});
    states[145] = new State(-13);
    states[146] = new State(new int[]{90,147});
    states[147] = new State(new int[]{64,149,65,150,62,151,63,152,66,153},new int[]{-13,148});
    states[148] = new State(-15);
    states[149] = new State(-17);
    states[150] = new State(-18);
    states[151] = new State(-19);
    states[152] = new State(-20);
    states[153] = new State(-21);
    states[154] = new State(new int[]{90,155});
    states[155] = new State(new int[]{64,149,65,150,62,151,63,152,66,153},new int[]{-13,156});
    states[156] = new State(-16);
    states[157] = new State(-14);
    states[158] = new State(new int[]{59,159});
    states[159] = new State(-8);
    states[160] = new State(new int[]{68,161,64,171,94,25,87,26,95,27},new int[]{-15,162,-12,172});
    states[161] = new State(-27);
    states[162] = new State(new int[]{92,163,44,168});
    states[163] = new State(new int[]{68,164});
    states[164] = new State(new int[]{90,165,59,-29});
    states[165] = new State(new int[]{81,166});
    states[166] = new State(new int[]{64,167});
    states[167] = new State(-30);
    states[168] = new State(new int[]{64,169,94,25,87,26,95,27},new int[]{-12,170});
    states[169] = new State(-31);
    states[170] = new State(-32);
    states[171] = new State(-33);
    states[172] = new State(-34);
    states[173] = new State(new int[]{68,174});
    states[174] = new State(-28);
    states[175] = new State(new int[]{59,176});
    states[176] = new State(-9);
    states[177] = new State(new int[]{82,178});
    states[178] = new State(new int[]{92,179});
    states[179] = new State(new int[]{81,180});
    states[180] = new State(new int[]{64,181});
    states[181] = new State(-24);
    states[182] = new State(new int[]{82,183});
    states[183] = new State(-25);
    states[184] = new State(new int[]{82,185});
    states[185] = new State(new int[]{90,186});
    states[186] = new State(new int[]{81,187});
    states[187] = new State(new int[]{64,188});
    states[188] = new State(-26);
    states[189] = new State(-10);
    states[190] = new State(-3);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,61});
    rules[2] = new Rule(-1, new int[]{-1,-3});
    rules[3] = new Rule(-1, new int[]{-3});
    rules[4] = new Rule(-3, new int[]{-4,59});
    rules[5] = new Rule(-3, new int[]{-5,59});
    rules[6] = new Rule(-3, new int[]{-6,59});
    rules[7] = new Rule(-3, new int[]{-7,59});
    rules[8] = new Rule(-3, new int[]{-8,59});
    rules[9] = new Rule(-3, new int[]{-9,59});
    rules[10] = new Rule(-3, new int[]{60});
    rules[11] = new Rule(-7, new int[]{103,-10,93,68});
    rules[12] = new Rule(-7, new int[]{103,-10,93,68,100,84,64});
    rules[13] = new Rule(-10, new int[]{-10,44,-11});
    rules[14] = new Rule(-10, new int[]{-11});
    rules[15] = new Rule(-11, new int[]{-12,90,-13});
    rules[16] = new Rule(-11, new int[]{64,90,-13});
    rules[17] = new Rule(-13, new int[]{64});
    rules[18] = new Rule(-13, new int[]{65});
    rules[19] = new Rule(-13, new int[]{62});
    rules[20] = new Rule(-13, new int[]{63});
    rules[21] = new Rule(-13, new int[]{66});
    rules[22] = new Rule(-14, new int[]{63});
    rules[23] = new Rule(-14, new int[]{62});
    rules[24] = new Rule(-9, new int[]{107,82,92,81,64});
    rules[25] = new Rule(-9, new int[]{108,82});
    rules[26] = new Rule(-9, new int[]{109,82,90,81,64});
    rules[27] = new Rule(-8, new int[]{105,68});
    rules[28] = new Rule(-8, new int[]{106,68});
    rules[29] = new Rule(-8, new int[]{105,-15,92,68});
    rules[30] = new Rule(-8, new int[]{105,-15,92,68,90,81,64});
    rules[31] = new Rule(-15, new int[]{-15,44,64});
    rules[32] = new Rule(-15, new int[]{-15,44,-12});
    rules[33] = new Rule(-15, new int[]{64});
    rules[34] = new Rule(-15, new int[]{-12});
    rules[35] = new Rule(-4, new int[]{102,-16});
    rules[36] = new Rule(-4, new int[]{68,-17,-16});
    rules[37] = new Rule(-16, new int[]{-18});
    rules[38] = new Rule(-16, new int[]{-18,64});
    rules[39] = new Rule(-16, new int[]{-18,85,-19});
    rules[40] = new Rule(-5, new int[]{104,-20});
    rules[41] = new Rule(-5, new int[]{104,83,64});
    rules[42] = new Rule(-5, new int[]{68,69,-20});
    rules[43] = new Rule(-20, new int[]{88,-18,64});
    rules[44] = new Rule(-20, new int[]{88,-18,86,64});
    rules[45] = new Rule(-20, new int[]{88,-18,86,64,77,87,64});
    rules[46] = new Rule(-6, new int[]{89,68,90,68});
    rules[47] = new Rule(-6, new int[]{91,68,92,68});
    rules[48] = new Rule(-19, new int[]{-19,77,-21});
    rules[49] = new Rule(-19, new int[]{-19,78,-21});
    rules[50] = new Rule(-19, new int[]{-21});
    rules[51] = new Rule(-21, new int[]{});
    rules[52] = new Rule(-21, new int[]{-22});
    rules[53] = new Rule(-21, new int[]{-23});
    rules[54] = new Rule(-21, new int[]{-24});
    rules[55] = new Rule(-21, new int[]{-25});
    rules[56] = new Rule(-21, new int[]{-26});
    rules[57] = new Rule(-21, new int[]{-27});
    rules[58] = new Rule(-22, new int[]{-12,-17,64});
    rules[59] = new Rule(-22, new int[]{-12,-17,66});
    rules[60] = new Rule(-22, new int[]{-12,-28,64});
    rules[61] = new Rule(-12, new int[]{94});
    rules[62] = new Rule(-12, new int[]{87});
    rules[63] = new Rule(-12, new int[]{95});
    rules[64] = new Rule(-23, new int[]{97,-17,64});
    rules[65] = new Rule(-23, new int[]{97,-28,64});
    rules[66] = new Rule(-23, new int[]{98,-17,-14});
    rules[67] = new Rule(-23, new int[]{98,-29,-14});
    rules[68] = new Rule(-24, new int[]{96,-17,80});
    rules[69] = new Rule(-24, new int[]{96,-17,64});
    rules[70] = new Rule(-24, new int[]{96,-28,64});
    rules[71] = new Rule(-24, new int[]{96,-17,66});
    rules[72] = new Rule(-24, new int[]{96,70,67});
    rules[73] = new Rule(-24, new int[]{96,69,67});
    rules[74] = new Rule(-24, new int[]{96,-25});
    rules[75] = new Rule(-24, new int[]{96,-22});
    rules[76] = new Rule(-26, new int[]{99,-25});
    rules[77] = new Rule(-26, new int[]{99,-22});
    rules[78] = new Rule(-25, new int[]{64,-17,62});
    rules[79] = new Rule(-25, new int[]{64,-29,62});
    rules[80] = new Rule(-25, new int[]{64,-17,63});
    rules[81] = new Rule(-25, new int[]{64,-29,63});
    rules[82] = new Rule(-25, new int[]{64,-17,64});
    rules[83] = new Rule(-25, new int[]{64,-28,64});
    rules[84] = new Rule(-25, new int[]{64,-17,65});
    rules[85] = new Rule(-25, new int[]{64,-17,66});
    rules[86] = new Rule(-25, new int[]{64,70,67});
    rules[87] = new Rule(-25, new int[]{64,69,67});
    rules[88] = new Rule(-27, new int[]{101,-30,68});
    rules[89] = new Rule(-30, new int[]{110});
    rules[90] = new Rule(-30, new int[]{111});
    rules[91] = new Rule(-30, new int[]{112});
    rules[92] = new Rule(-30, new int[]{113});
    rules[93] = new Rule(-30, new int[]{114});
    rules[94] = new Rule(-30, new int[]{115});
    rules[95] = new Rule(-30, new int[]{116});
    rules[96] = new Rule(-30, new int[]{117});
    rules[97] = new Rule(-30, new int[]{118});
    rules[98] = new Rule(-30, new int[]{119});
    rules[99] = new Rule(-30, new int[]{120});
    rules[100] = new Rule(-30, new int[]{121});
    rules[101] = new Rule(-30, new int[]{75});
    rules[102] = new Rule(-30, new int[]{123});
    rules[103] = new Rule(-30, new int[]{124});
    rules[104] = new Rule(-17, new int[]{69});
    rules[105] = new Rule(-17, new int[]{70});
    rules[106] = new Rule(-29, new int[]{71});
    rules[107] = new Rule(-29, new int[]{72});
    rules[108] = new Rule(-29, new int[]{73});
    rules[109] = new Rule(-29, new int[]{74});
    rules[110] = new Rule(-28, new int[]{75});
    rules[111] = new Rule(-28, new int[]{76});
    rules[112] = new Rule(-18, new int[]{79});
    rules[113] = new Rule(-18, new int[]{80});
    rules[114] = new Rule(-18, new int[]{97});
    rules[115] = new Rule(-18, new int[]{99});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 11: // attr_setting -> SET, value_setting_list, FOR, IDENTIFIER
#line 113 "Parser.y"
{EvaluateSetExpression(ValueStack[ValueStack.Depth-1].strVal, ((List<Expression>)(ValueStack[ValueStack.Depth-3].val)));}
        break;
      case 12: // attr_setting -> SET, value_setting_list, FOR, IDENTIFIER, IN, PROPERTY_SET, 
               //                 STRING
#line 114 "Parser.y"
{EvaluateSetExpression(ValueStack[ValueStack.Depth-4].strVal, ((List<Expression>)(ValueStack[ValueStack.Depth-6].val)), ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 13: // value_setting_list -> value_setting_list, ',', value_setting
#line 118 "Parser.y"
{((List<Expression>)(ValueStack[ValueStack.Depth-3].val)).Add((Expression)(ValueStack[ValueStack.Depth-1].val)); CurrentSemanticValue.val = ValueStack[ValueStack.Depth-3].val;}
        break;
      case 14: // value_setting_list -> value_setting
#line 119 "Parser.y"
{CurrentSemanticValue.val = new List<Expression>(){((Expression)(ValueStack[ValueStack.Depth-1].val))};}
        break;
      case 15: // value_setting -> attribute, TO, value
#line 123 "Parser.y"
{CurrentSemanticValue.val = GenerateSetExpression(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].val);}
        break;
      case 16: // value_setting -> STRING, TO, value
#line 124 "Parser.y"
{CurrentSemanticValue.val = GenerateSetExpression(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].val);}
        break;
      case 17: // value -> STRING
#line 128 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].strVal;}
        break;
      case 18: // value -> BOOLEAN
#line 129 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].boolVal;}
        break;
      case 19: // value -> INTEGER
#line 130 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].intVal;}
        break;
      case 20: // value -> DOUBLE
#line 131 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].doubleVal;}
        break;
      case 21: // value -> NONDEF
#line 132 "Parser.y"
{CurrentSemanticValue.val = null;}
        break;
      case 22: // num_value -> DOUBLE
#line 136 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].doubleVal;}
        break;
      case 23: // num_value -> INTEGER
#line 137 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].intVal;}
        break;
      case 24: // model_actions -> OPEN, MODEL, FROM, FILE, STRING
#line 141 "Parser.y"
{OpenModel(ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 25: // model_actions -> CLOSE, MODEL
#line 142 "Parser.y"
{CloseModel();}
        break;
      case 26: // model_actions -> SAVE, MODEL, TO, FILE, STRING
#line 143 "Parser.y"
{SaveModel(ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 27: // variables_actions -> DUMP, IDENTIFIER
#line 147 "Parser.y"
{DumpIdentifier(ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 28: // variables_actions -> CLEAR, IDENTIFIER
#line 148 "Parser.y"
{ClearIdentifier(ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 29: // variables_actions -> DUMP, string_list, FROM, IDENTIFIER
#line 149 "Parser.y"
{DumpAttributes(ValueStack[ValueStack.Depth-1].strVal, ((List<string>)(ValueStack[ValueStack.Depth-3].val)));}
        break;
      case 30: // variables_actions -> DUMP, string_list, FROM, IDENTIFIER, TO, FILE, STRING
#line 150 "Parser.y"
{DumpAttributes(ValueStack[ValueStack.Depth-4].strVal, ((List<string>)(ValueStack[ValueStack.Depth-6].val)), ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 31: // string_list -> string_list, ',', STRING
#line 154 "Parser.y"
{((List<string>)(ValueStack[ValueStack.Depth-3].val)).Add(ValueStack[ValueStack.Depth-1].strVal); CurrentSemanticValue.val = ValueStack[ValueStack.Depth-3].val;}
        break;
      case 32: // string_list -> string_list, ',', attribute
#line 155 "Parser.y"
{((List<string>)(ValueStack[ValueStack.Depth-3].val)).Add(ValueStack[ValueStack.Depth-1].strVal); CurrentSemanticValue.val = ValueStack[ValueStack.Depth-3].val;}
        break;
      case 33: // string_list -> STRING
#line 156 "Parser.y"
{CurrentSemanticValue.val = new List<string>(){ValueStack[ValueStack.Depth-1].strVal};}
        break;
      case 34: // string_list -> attribute
#line 157 "Parser.y"
{CurrentSemanticValue.val = new List<string>(){ValueStack[ValueStack.Depth-1].strVal};}
        break;
      case 35: // selection -> SELECT, selection_statement
#line 161 "Parser.y"
{Variables.Set("$$", ((IEnumerable<IPersistIfcEntity>)(ValueStack[ValueStack.Depth-1].val)));}
        break;
      case 36: // selection -> IDENTIFIER, op_bool, selection_statement
#line 162 "Parser.y"
{AddOrRemoveFromSelection(ValueStack[ValueStack.Depth-3].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)), ValueStack[ValueStack.Depth-1].val);}
        break;
      case 37: // selection_statement -> object
#line 166 "Parser.y"
{CurrentSemanticValue.val = Select(ValueStack[ValueStack.Depth-1].typeVal);}
        break;
      case 38: // selection_statement -> object, STRING
#line 167 "Parser.y"
{CurrentSemanticValue.val = Select(ValueStack[ValueStack.Depth-2].typeVal, ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 39: // selection_statement -> object, WHERE, conditions
#line 168 "Parser.y"
{CurrentSemanticValue.val = Select(ValueStack[ValueStack.Depth-3].typeVal, ((Expression)(ValueStack[ValueStack.Depth-1].val)));}
        break;
      case 40: // creation -> CREATE, creation_statement
#line 172 "Parser.y"
{Variables.Set("$$", ((IPersistIfcEntity)(ValueStack[ValueStack.Depth-1].val)));}
        break;
      case 41: // creation -> CREATE, CLASSIFICATION, STRING
#line 173 "Parser.y"
{CreateClassification(ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 42: // creation -> IDENTIFIER, OP_EQ, creation_statement
#line 174 "Parser.y"
{Variables.Set(ValueStack[ValueStack.Depth-3].strVal, ((IPersistIfcEntity)(ValueStack[ValueStack.Depth-1].val)));}
        break;
      case 43: // creation_statement -> NEW, object, STRING
#line 178 "Parser.y"
{CurrentSemanticValue.val = CreateObject(ValueStack[ValueStack.Depth-2].typeVal, ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 44: // creation_statement -> NEW, object, WITH_NAME, STRING
#line 179 "Parser.y"
{CurrentSemanticValue.val = CreateObject(ValueStack[ValueStack.Depth-3].typeVal, ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 45: // creation_statement -> NEW, object, WITH_NAME, STRING, OP_AND, DESCRIPTION, 
               //                       STRING
#line 180 "Parser.y"
{CurrentSemanticValue.val = CreateObject(ValueStack[ValueStack.Depth-6].typeVal, ValueStack[ValueStack.Depth-4].strVal, ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 46: // addition -> ADD, IDENTIFIER, TO, IDENTIFIER
#line 184 "Parser.y"
{AddOrRemove(Tokens.ADD, ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 47: // addition -> REMOVE, IDENTIFIER, FROM, IDENTIFIER
#line 185 "Parser.y"
{AddOrRemove(Tokens.REMOVE, ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 48: // conditions -> conditions, OP_AND, condition
#line 189 "Parser.y"
{CurrentSemanticValue.val = Expression.AndAlso(((Expression)(ValueStack[ValueStack.Depth-3].val)), ((Expression)(ValueStack[ValueStack.Depth-1].val)));}
        break;
      case 49: // conditions -> conditions, OP_OR, condition
#line 190 "Parser.y"
{CurrentSemanticValue.val = Expression.OrElse(((Expression)(ValueStack[ValueStack.Depth-3].val)), ((Expression)(ValueStack[ValueStack.Depth-1].val)));}
        break;
      case 50: // conditions -> condition
#line 191 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].val;}
        break;
      case 52: // condition -> attributeCondition
#line 196 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].val;}
        break;
      case 53: // condition -> materialCondition
#line 197 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].val;}
        break;
      case 54: // condition -> typeCondition
#line 198 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].val;}
        break;
      case 55: // condition -> propertyCondition
#line 199 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].val;}
        break;
      case 56: // condition -> groupCondition
#line 200 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].val;}
        break;
      case 57: // condition -> spatialCondition
#line 201 "Parser.y"
{CurrentSemanticValue.val = ValueStack[ValueStack.Depth-1].val;}
        break;
      case 58: // attributeCondition -> attribute, op_bool, STRING
#line 205 "Parser.y"
{CurrentSemanticValue.val = GenerateAttributeCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 59: // attributeCondition -> attribute, op_bool, NONDEF
#line 206 "Parser.y"
{CurrentSemanticValue.val = GenerateAttributeCondition(ValueStack[ValueStack.Depth-3].strVal, null, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 60: // attributeCondition -> attribute, op_cont, STRING
#line 207 "Parser.y"
{CurrentSemanticValue.val = GenerateAttributeCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 61: // attribute -> NAME
#line 211 "Parser.y"
{CurrentSemanticValue.strVal = "Name";}
        break;
      case 62: // attribute -> DESCRIPTION
#line 212 "Parser.y"
{CurrentSemanticValue.strVal = "Description";}
        break;
      case 63: // attribute -> PREDEFINED_TYPE
#line 213 "Parser.y"
{CurrentSemanticValue.strVal = "PredefinedType";}
        break;
      case 64: // materialCondition -> MATERIAL, op_bool, STRING
#line 217 "Parser.y"
{CurrentSemanticValue.val = GenerateMaterialCondition(ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 65: // materialCondition -> MATERIAL, op_cont, STRING
#line 218 "Parser.y"
{CurrentSemanticValue.val = GenerateMaterialCondition(ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 66: // materialCondition -> THICKNESS, op_bool, num_value
#line 220 "Parser.y"
{CurrentSemanticValue.val = GenerateThicknessCondition(Convert.ToDouble(ValueStack[ValueStack.Depth-1].val), ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 67: // materialCondition -> THICKNESS, op_num_rel, num_value
#line 221 "Parser.y"
{CurrentSemanticValue.val = GenerateThicknessCondition(Convert.ToDouble(ValueStack[ValueStack.Depth-1].val), ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 68: // typeCondition -> TYPE, op_bool, PRODUCT_TYPE
#line 225 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeObjectTypeCondition(ValueStack[ValueStack.Depth-1].typeVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 69: // typeCondition -> TYPE, op_bool, STRING
#line 226 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeObjectNameCondition(ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 70: // typeCondition -> TYPE, op_cont, STRING
#line 227 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeObjectNameCondition(ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 71: // typeCondition -> TYPE, op_bool, NONDEF
#line 228 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeObjectTypeCondition(null, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 72: // typeCondition -> TYPE, OP_NEQ, DEFINED
#line 229 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeObjectTypeCondition(null, Tokens.OP_EQ);}
        break;
      case 73: // typeCondition -> TYPE, OP_EQ, DEFINED
#line 230 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeObjectTypeCondition(null, Tokens.OP_NEQ);}
        break;
      case 74: // typeCondition -> TYPE, propertyCondition
#line 231 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeCondition((Expression)(ValueStack[ValueStack.Depth-1].val));}
        break;
      case 75: // typeCondition -> TYPE, attributeCondition
#line 232 "Parser.y"
{CurrentSemanticValue.val = GenerateTypeCondition((Expression)(ValueStack[ValueStack.Depth-1].val));}
        break;
      case 76: // groupCondition -> GROUP, propertyCondition
#line 236 "Parser.y"
{CurrentSemanticValue.val = GenerateGroupCondition((Expression)(ValueStack[ValueStack.Depth-1].val));}
        break;
      case 77: // groupCondition -> GROUP, attributeCondition
#line 237 "Parser.y"
{CurrentSemanticValue.val = GenerateGroupCondition((Expression)(ValueStack[ValueStack.Depth-1].val));}
        break;
      case 78: // propertyCondition -> STRING, op_bool, INTEGER
#line 241 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].intVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 79: // propertyCondition -> STRING, op_num_rel, INTEGER
#line 242 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].intVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 80: // propertyCondition -> STRING, op_bool, DOUBLE
#line 244 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].doubleVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 81: // propertyCondition -> STRING, op_num_rel, DOUBLE
#line 245 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].doubleVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 82: // propertyCondition -> STRING, op_bool, STRING
#line 247 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 83: // propertyCondition -> STRING, op_cont, STRING
#line 248 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].strVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 84: // propertyCondition -> STRING, op_bool, BOOLEAN
#line 250 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, ValueStack[ValueStack.Depth-1].boolVal, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 85: // propertyCondition -> STRING, op_bool, NONDEF
#line 251 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, null, ((Tokens)(ValueStack[ValueStack.Depth-2].val)));}
        break;
      case 86: // propertyCondition -> STRING, OP_NEQ, DEFINED
#line 252 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, null, Tokens.OP_EQ);}
        break;
      case 87: // propertyCondition -> STRING, OP_EQ, DEFINED
#line 253 "Parser.y"
{CurrentSemanticValue.val = GeneratePropertyCondition(ValueStack[ValueStack.Depth-3].strVal, null, Tokens.OP_NEQ);}
        break;
      case 88: // spatialCondition -> IT, op_spatial, IDENTIFIER
#line 257 "Parser.y"
{CurrentSemanticValue.val = GenerateSpatialCondition(((Tokens)(ValueStack[ValueStack.Depth-2].val)), ValueStack[ValueStack.Depth-1].strVal);}
        break;
      case 89: // op_spatial -> NORTH_OF
#line 261 "Parser.y"
{CurrentSemanticValue.val = Tokens.NORTH_OF			;}
        break;
      case 90: // op_spatial -> SOUTH_OF
#line 262 "Parser.y"
{CurrentSemanticValue.val = Tokens.SOUTH_OF			;}
        break;
      case 91: // op_spatial -> WEST_OF
#line 263 "Parser.y"
{CurrentSemanticValue.val = Tokens.WEST_OF			;}
        break;
      case 92: // op_spatial -> EAST_OF
#line 264 "Parser.y"
{CurrentSemanticValue.val = Tokens.EAST_OF			;}
        break;
      case 93: // op_spatial -> ABOVE
#line 265 "Parser.y"
{CurrentSemanticValue.val = Tokens.ABOVE				;}
        break;
      case 94: // op_spatial -> BELOW
#line 266 "Parser.y"
{CurrentSemanticValue.val = Tokens.BELOW				;}
        break;
      case 95: // op_spatial -> SPATIALLY_EQUALS
#line 267 "Parser.y"
{CurrentSemanticValue.val = Tokens.SPATIALLY_EQUALS	;}
        break;
      case 96: // op_spatial -> DISJOINT
#line 268 "Parser.y"
{CurrentSemanticValue.val = Tokens.DISJOINT			;}
        break;
      case 97: // op_spatial -> INTERSECTS
#line 269 "Parser.y"
{CurrentSemanticValue.val = Tokens.INTERSECTS			;}
        break;
      case 98: // op_spatial -> TOUCHES
#line 270 "Parser.y"
{CurrentSemanticValue.val = Tokens.TOUCHES			;}
        break;
      case 99: // op_spatial -> CROSSES
#line 271 "Parser.y"
{CurrentSemanticValue.val = Tokens.CROSSES			;}
        break;
      case 100: // op_spatial -> WITHIN
#line 272 "Parser.y"
{CurrentSemanticValue.val = Tokens.WITHIN				;}
        break;
      case 101: // op_spatial -> OP_CONTAINS
#line 273 "Parser.y"
{CurrentSemanticValue.val = Tokens.SPATIALLY_CONTAINS	;}
        break;
      case 102: // op_spatial -> OVERLAPS
#line 274 "Parser.y"
{CurrentSemanticValue.val = Tokens.OVERLAPS			;}
        break;
      case 103: // op_spatial -> RELATE
#line 275 "Parser.y"
{CurrentSemanticValue.val = Tokens.RELATE				;}
        break;
      case 104: // op_bool -> OP_EQ
#line 279 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_EQ;}
        break;
      case 105: // op_bool -> OP_NEQ
#line 280 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_NEQ;}
        break;
      case 106: // op_num_rel -> OP_GT
#line 284 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_GT;}
        break;
      case 107: // op_num_rel -> OP_LT
#line 285 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_LT;}
        break;
      case 108: // op_num_rel -> OP_GTE
#line 286 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_GTE;}
        break;
      case 109: // op_num_rel -> OP_LTQ
#line 287 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_LTQ;}
        break;
      case 110: // op_cont -> OP_CONTAINS
#line 291 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_CONTAINS;}
        break;
      case 111: // op_cont -> OP_NOT_CONTAINS
#line 292 "Parser.y"
{CurrentSemanticValue.val = Tokens.OP_NOT_CONTAINS;}
        break;
      case 112: // object -> PRODUCT
#line 296 "Parser.y"
{CurrentSemanticValue.typeVal = ValueStack[ValueStack.Depth-1].typeVal;}
        break;
      case 113: // object -> PRODUCT_TYPE
#line 297 "Parser.y"
{CurrentSemanticValue.typeVal = ValueStack[ValueStack.Depth-1].typeVal;}
        break;
      case 114: // object -> MATERIAL
#line 298 "Parser.y"
{CurrentSemanticValue.typeVal = ValueStack[ValueStack.Depth-1].typeVal;}
        break;
      case 115: // object -> GROUP
#line 299 "Parser.y"
{CurrentSemanticValue.typeVal = ValueStack[ValueStack.Depth-1].typeVal;}
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
}