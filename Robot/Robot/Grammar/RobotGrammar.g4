grammar RobotGrammar;


program: progInstructionSet;

progInstructionSet: progInstruction*;

progInstruction: functionDef |
                 instruction;


instructionSet: instruction+;
instruction: moveInstruction |
             turnInstruction |
             loopInstruction |
             pickUpInstruction |
             dropInstruction |
             functionCall;
// loops
loopInstruction: LOOPCMD repeatCnt 
                     instructionSet 
                 LOOPENDCMD;

// function definitions
functionDef: FUNCTIONCMD functionName BRACKET1 parameterDefList? BRACKET2
                  instructionSet
             FUNCTIONENDCMD;
parameterDefList: parameterDef (COMMA parameterDef)*;
parameterDef: paramType parameterName;
// function calls
functionCall: CALLCMD functionName BRACKET1 parameterList? BRACKET2;
parameterList: parameter (COMMA parameter)*;
parameter: parameterName|intParameter|dirParameter;

// simple instructions
moveInstruction: MOVECMD moveAmount;
turnInstruction: TURNCMD dir;
pickUpInstruction: PICKUPCMD;
dropInstruction: DROPCMD itemId;


functionName: ID;
parameterName: ID;
// prameter types for declaring a function
paramType: TYPEINT|TYPEDIR;
// parameters for calling a function
dirParameter: dir;
intParameter: INT;

dir: leftDir|rightDir|parameterName;
leftDir: LEFT|MINUS;
rightDir: RIGHT|PLUS;
moveAmount: INT|parameterName;
repeatCnt: INT|parameterName;
itemId: INT;

// Lexer rules

FUNCTIONCMD: 'function';
FUNCTIONENDCMD: 'end function';
CALLCMD: 'call';
LOOPENDCMD: 'end loop';
LOOPWHILECMD: 'loop while';
LOOPCMD: 'loop';
MOVECMD:  'move'; //[mM][oO][vV][eE]; 
TURNCMD: 'turn';
PICKUPCMD: 'pick up'|'pickup';
DROPCMD: 'drop';
FREECMD: 'free';
WALLCMD: 'wall';
TYPEINT: 'int';
TYPEDIR: 'dir';

NOT: 'not'|'!';
AND: 'and'|'&&';
OR: 'or'|'||';
LEFT: 'left';
RIGHT: 'right';
PLUS: '+';
MINUS: '-';
BRACKET1: '(';
BRACKET2: ')';
COMMA: ',';
INT: [0-9]+;
ID: [a-zA-Z][a-zA-Z0-9_]*;

WS: 	(' ' | '\t' | '\n' | '\r') -> skip;
