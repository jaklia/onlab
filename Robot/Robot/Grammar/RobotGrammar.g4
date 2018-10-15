grammar RobotGrammar;

program: functionDefinitions 
         instructionSet;
functionDefinitions: functionDef*;
instructionSet: instruction+;
instruction: moveInstruction |
             turnInstruction |
             loopWhileInstruction |
             loopInstruction |
             pickUpInstruction |
             dropInstruction |
             functionCall;

loopInstruction: LOOPCMD repeatCnt 
                     instructionSet 
                 LOOPENDCMD;
loopWhileInstruction: LOOPWHILECMD BRACKET1 condition BRACKET2 
                          instructionSet 
                      LOOPENDCMD;
moveInstruction: MOVECMD moveAmount;
turnInstruction: TURNCMD dir;
pickUpInstruction: PICKUPCMD;
dropInstruction: DROPCMD itemId;

functionDef: FUNCTIONCMD functionName
                  instructionSet
             FUNCTIONENDCMD;
functionCall: functionName BRACKET1 BRACKET2;


condition: (NOT? (FREECMD|WALLCMD));    //   A && B,  A || B  ???

dir: leftDir|rightDir;
leftDir: LEFT|MINUS;
rightDir: RIGHT|PLUS;
moveAmount: INT;
repeatCnt: INT;
itemId: INT;
functionName: ID;

FUNCTIONCMD: 'function';
FUNCTIONENDCMD: 'end function';
LOOPENDCMD: 'end loop';
LOOPWHILECMD: 'loop while';
LOOPCMD: 'loop';
MOVECMD:  'move'; //[mM][oO][vV][eE]; 
TURNCMD: 'turn';
PICKUPCMD: 'pick up'|'pickup';
DROPCMD: 'drop';
FREECMD: 'free';
WALLCMD: 'wall';

NOT: 'not'|'!';
AND: 'and'|'&&';
OR: 'or'|'||';
LEFT: 'left';
RIGHT: 'right';
PLUS: '+';
MINUS: '-';
BRACKET1: '(';
BRACKET2: ')';
INT: [0-9]+;
ID: [a-zA-Z][a-zA-Z0-9_]*;

WS: 	(' ' | '\t' | '\n' | '\r') -> skip;
