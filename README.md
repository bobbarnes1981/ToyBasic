# ToyBasic

[![Build Status](https://travis-ci.org/bobbarnes1981/ToyBasic.svg?branch=master)](https://travis-ci.org/bobbarnes1981/ToyBasic)

An attempt to write a simple basic interpreter.

http://bobbarnes1981.github.io/ToyBasic/

https://github.com/bobbarnes1981/ToyBasic/wiki

##Examples - Program commands

###Print - hello world

10 Print "Hello World"

Run

###If - comparison

10 Let var$ = 8

20 If 8 == var$ THEN Print "are equal"

Run

###Goto - 

10 Print "forever"

20 Goto 10

Run

###For - 1 to 10

10 For x$ = 1 To 10 Step 1

20 Print x$

30 Next x$

Run

###Expressions - addition

10 Let a$ = 8

20 Let b$ = 9

30 Print a$ + b$

40 Print 8 + 9

Run

##Examples - System commands

###Renumber - make line numbers start at ten and increment in 10s

Renumber

###List - show the code in the line buffer

List

###Exit - exit the interpreter

Exit
