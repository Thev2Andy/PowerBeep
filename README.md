# PowerBeep
A kind of _programming language_ (it's a beep sequencer by text) made in C#, that can sequence console beeps.

You can read more in the documentation [here](https://github.com/Thev2Andy/PowerBeep/wiki).

# Features
* Can be integrated in any program, by calling only one function.
```cs
PowerBeep.Interpreter.Interpret(Code);
```

* Easy, bash-like syntax.
```
beep 1
wait 450 // Anything beyond the keyword and value is ignored.
beep
```

# Limitations
* Not entirely cross-platform. (Doesn't run on web browsers, Android, iOS, tvOS)
* No conditional checks.
* Designed as a small-scale language.

# Dependencies
* [PowerLog](https://github.com/Thev2Andy/PowerLog) logging library.
