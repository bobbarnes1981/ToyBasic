# ToyBasic

An attempt to write a simple basic interpreter.

##Todo:

Implement Help

Gosub/Return?

##Example:

###print hello world forever

10 Print "Hello World"

20 Goto 10

Run

###two ways to print the sum of 8 and 9

10 Let $a = 8

20 Let $b = 9

30 Print $a + $b

40 Print 8 + 9

Run

###list the code

List

###exit the interpreter

Exit

###if statement

10 Let $var = 8

20 If 8 == $var THEN Print "are equal"

Run

###for statement (1 to 10)

10 For $x = 1 To 10 Step 1

20 Print $x

30 Next $x

Run
