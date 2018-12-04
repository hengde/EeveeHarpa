using System.Collections.Generic;

/// <summary>
/// Implements the ShuffleBag data collection
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class ShuffleBag<T>
{
	private System.Random random = new System.Random();
	private List<T> data;

	private T currentItem;
	private int currentPosition = -1;

	public int Capacity
	{
		get { return data.Capacity; }
	}

	public int Size
	{
		get { return data.Count; }
	}

	public ShuffleBag()
	{
		data = new List<T>();
	}

	public ShuffleBag(int initCapacity)
	{
		data = new List<T>(initCapacity);
	}

	/// <summary>
	/// Add a number of items to the bad.
	/// </summary>
	/// <param name="item"> item to add. </param>
	/// <param name="amount"> amount of items to add. </param>
	public void Add(T item, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			data.Add(item);
		}

		currentPosition = Size - 1;
	}

	/// <summary>
	/// Get an item from the bag.
	/// </summary>
	/// <returns> A randomly selected item from the bag. </returns>
	public T Next()
	{
		// If currentPosition is less than one, we reset it to point to the end of the list
		// and return the first item from the bag. (This covers the situation where we've 
		// traversed through all the items and now wish to start again.)
		if (currentPosition < 1)
		{
			currentPosition = Size - 1;
			currentItem = data[0];

			return currentItem;
		}

		int pos = random.Next(currentPosition);
		currentItem = data[pos];
		data[pos] = data[currentPosition];
		data[currentPosition] = currentItem;
		currentPosition--;

		return currentItem;
	}
}