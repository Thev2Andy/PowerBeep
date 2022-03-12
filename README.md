# PowerBeep
An interpreted programming language made in C#, that can sequence console beeps.

# Features
* Can be integrated in any program, by calling only one function.
```cs
PowerBeep.Interpreter.Interpret(Code);
```

* Toddler-level bash-like syntax.
```
beep 1
wait 450 // Anything beyond the keyword and value is ignored.
beep
```
# Dependencies
* [PowerLog](https://github.com/Thev2Andy/PowerLog) logging library.
