10 Clear
20 For $i = 1 To 3 Step 1
30 For $j = 1 To 3 Step 1
40 Print $i
50 Print $j
60 Next $j
70 Next $i
80 Goto 100
90 Print "Goto Failed!"
100 Let $number = 8
110 If $number == 8 Then Print "$number is 8"
120 Let $x = 0
130 Let $x = $x + 1
140 Print $x
150 If $x == 0 Then Goto 130
160 Rem This is a test
170 Dim $a[10]
180 Let $a[0] = "bob"
190 Let $a[1] = 808
200 Print $a[0] + "text"
210 Print $a[1] + 1
220 Print "Enter a word"
230 Input $y
240 Print $y
250 Gosub 280
260 Print "Return from Gosub"
270 Goto 310
280 Rem Subroutine
290 Print "Subroutine"
300 Return
310 Print "End"