﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem, CheckerProblem.NoProblem)]
[CodeFixTests.TestThis]
public class AwaitForeach2_Fine
{
	public async Task FooBar()
	{
		await foreach (var item in (await TestsBase.F<IAsyncEnumerable<int>>(default).ConfigureAwait(false)).ConfigureAwait(false))
		{ }
	}
}
