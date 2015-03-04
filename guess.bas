10 Rem Guess the Number
20 Print "Guess the Number"
30 Let number$ = Random$ % 10
40 Print "Enter your guess"
50 Input guess$
60 If guess$ < number$ Then Goto 90
70 If guess$ > number$ Then Goto 120
80 Goto 150
90 Rem Too Low 
100 Print "Too Low"
110 Goto 40
120 Rem Too High
130 Print "Too High"
140 Goto 40
150 Rem Correct
160 Print "Correct!"
170 Rem Again
180 Print "Again (y/n)?"
190 Input again$
200 If again$ == "y" Then Goto 10
210 If again$ != "n" Then Goto 170
220 Print "bye bye"