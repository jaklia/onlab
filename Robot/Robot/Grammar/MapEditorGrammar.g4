grammar MapEditorGrammar;

/*
 * Parser Rules
 */
buildMap: map
          options;

map: MAPCMD height width;

options: mapOptionRow*;

mapOptionRow: walls |
              start |
              finish |
              key;

walls: wall+;
wall: WALLCMD row col;
start: STARTCMD row col;
finish: FINISHCMD row col;
key: KEYCMD row col;

height: INT;
width: INT;
col: INT;
row: INT;


/*
 * Lexer Rules
 */

MAPCMD: 'map';
WALLCMD: 'wall';
KEYCMD: 'key';
STARTCMD: 'start';
FINISHCMD: 'finish';


INT: [0-9]+;
ID: [a-zA-Z][a-zA-Z0-9_]*;

WS: 	(' ' | '\t' | '\n' | '\r') -> skip;
