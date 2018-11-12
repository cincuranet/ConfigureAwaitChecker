﻿using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true }, new[] { false })]
public class ThrowAwait_Missing : TestClassBase
{
	public async Task FooBar()
	{
		throw await Exception();
	}
}
