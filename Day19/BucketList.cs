using System;
using System.Collections.Generic;
using System.Linq;

public class BucketList<T>
{
	private readonly List<List<T>> _buckets = new List<List<T>>();
	private readonly int _bucketSize;

	public BucketList(int bucketSize)
	{
		_bucketSize = bucketSize;
		_buckets.Add(new List<T>());
	}

	public int Count { get; private set; }

	public T this[int index]
	{
		get
		{
			var (list, i) = GetListAndIndex(index);
			return list[i];
		}
	}

	private (List<T> list, int i) GetListAndIndex(int index)
	{
		foreach (var bucket in _buckets)
		{
			if (index < bucket.Count)
			{
				return (bucket, index);
			}

			index -= bucket.Count;
		}

		throw new InvalidOperationException();
	}

	public void RemoveAt(int index)
	{
		var (list, i) = GetListAndIndex(index);
		list.RemoveAt(i);
		if (!list.Any())
		{
			_buckets.Remove(list);
		}
		Count--;
	}
	public void Add(T t)
	{
		if (_buckets.Last().Count >= _bucketSize)
		{
			_buckets.Add(new List<T>());
		}
		_buckets.Last().Add(t);
		Count++;
	}
}