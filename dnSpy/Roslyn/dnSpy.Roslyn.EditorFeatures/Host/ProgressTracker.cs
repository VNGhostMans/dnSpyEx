// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis.Shared.Utilities;

namespace dnSpy.Roslyn.EditorFeatures.Host {
	/// <summary>
	/// Utility class that can be used to track the progress of an operation in a threadsafe manner.
	/// </summary>
	internal class ProgressTracker : IProgressTracker {
		private int _completedItems;
		private int _totalItems;

		public string Description { get; set; }

		private readonly Action<int, int> _updateActionOpt;

		public ProgressTracker()
			: this(null) { }

		public ProgressTracker(Action<int, int> updateActionOpt) {
			_updateActionOpt = updateActionOpt;
		}

		public int CompletedItems => _completedItems;

		public int TotalItems => _totalItems;

		public void AddItems(int count) {
			Interlocked.Add(ref _totalItems, count);
			Update();
		}

		public void ItemCompleted() {
			Interlocked.Increment(ref _completedItems);
			Update();
		}

		public void Clear() {
			_totalItems = 0;
			_completedItems = 0;
			Update();
		}

		private void Update() {
			_updateActionOpt?.Invoke(_completedItems, _totalItems);
		}
	}
}
