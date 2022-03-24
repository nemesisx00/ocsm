#![allow(non_snake_case, non_upper_case_globals)]

use serde::{Serialize, Deserialize};

use std::fmt::
{
	Display,
	Formatter,
	Result,
};

/// The possible states for a Tracker's values.
#[derive(Clone, Copy, Debug, Deserialize, Eq, PartialEq, PartialOrd, Ord, Serialize)]
pub enum TrackerState
{
	Three,
	Two,
	One
}

/// A generic tracker for the various "Tracks" that are used
/// throughout Chronicles of Darkness.
#[derive(Clone, Default, Deserialize, PartialEq, Serialize)]
pub struct Tracker
{
	max: usize,
	pub values: Vec<TrackerState>
}

impl Tracker
{
	pub fn new(max: usize) -> Tracker
	{
		return Tracker
		{
			max: max,
			values: Vec::new()
		};
	}
	
	/// Add a new TrackerState to the Tracker.
	/// 
	/// Does not exceed the Tracker's `max` capacity.
	/// 
	/// The values are sorted every time a new value is added.
	pub fn add(&mut self, state: TrackerState)
	{
		if self.values.len() < self.max
		{
			match state
			{
				TrackerState::Three => self.values.push(TrackerState::Three),
				TrackerState::Two => self.values.push(TrackerState::Two),
				TrackerState::One => self.values.push(TrackerState::One),
			}
		}
		
		self.values.sort();
	}
	
	pub fn getMax(self) -> usize
	{
		return self.max;
	}
	
	pub fn getValue(self, index: usize) -> Option<TrackerState>
	{
		match self.values.get(index)
		{
			Some(ts) =>
			{
				match ts
				{
					TrackerState::One => { Some(TrackerState::One) }
					TrackerState::Two => { Some(TrackerState::Two) }
					TrackerState::Three => { Some(TrackerState::Three) }
				}
			}
			None => { None }
		}
	}
	
	/// Remove one value of `state` TrackerState.
	pub fn remove(&mut self, state: TrackerState)
	{
		match self.values.iter().position(|ts| *ts == state)
		{
			Some(index) => { self.values.remove(index); },
			None => {}
		}
		
		self.values.sort();
	}
	
	pub fn update(&mut self, state: TrackerState, index: usize)
	{
		match self.values.get(index)
		{
			Some(ts) =>
			{
				match ts
				{
					TrackerState::One => { self.replace(index, TrackerState::Two); }
					TrackerState::Two => { self.replace(index, TrackerState::Three); }
					TrackerState::Three => { self.remove(TrackerState::Three); }
				}
			}
			None => { self.add(state); }
		}
		
		self.values.sort();
	}
	
	pub fn updateMax(&mut self, max: usize)
	{
		self.max = max;
		
		if self.values.len() > max
		{
			for _ in 0..(self.values.len() - max)
			{
				self.values.pop().unwrap();
			}
		}
	}
	
	fn replace(&mut self, index: usize, new: TrackerState)
	{
		self.values.remove(index);
		self.values.push(new);
	}
}

impl Display for Tracker
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		let mut output = String::new();
		for val in self.values.iter()
		{
			if output.len() > 0
			{
				output += " ";
			}
			let s = match val
			{
				TrackerState::Three => "3",
				TrackerState::Two => "2",
				TrackerState::One => "1",
			};
			output += s;
		}
		
		let nonVals = self.max - self.values.len();
		if nonVals > 0
		{
			for _ in 0..nonVals
			{
				if output.len() > 0
				{
					output += " ";
				}
				output += "-"
			}
		}
		
		return write!(f, "{:?}", output);
	}
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn testAdd()
	{
		let max = 3;
		let mut t = Tracker::new(max);
		t.add(TrackerState::One);
		
		assert_eq!(1, t.values.len());
	}
	
	#[test]
	fn testRemove()
	{
		let max = 3;
		let mut t = Tracker::new(max);
		t.add(TrackerState::One);
		
		t.remove(TrackerState::Three);
		assert_eq!(1, t.values.len());
		
		t.remove(TrackerState::One);
		assert_eq!(0, t.values.len());
	}
	
	#[test]
	fn testValue()
	{
		let max = 3;
		let mut t = Tracker::new(max);
		t.add(TrackerState::One);
		t.add(TrackerState::Two);
		
		assert_eq!(2, t.values.len());
		assert_eq!(t.values.len(), t.value());
	}
	
	#[test]
	fn testUpdateMax()
	{
		let max = 3;
		let altMax = 2;
		
		assert_eq!(true, altMax < max);
		
		let mut t = Tracker::new(max);
		t.add(TrackerState::One);
		t.add(TrackerState::One);
		t.add(TrackerState::One);
		
		assert_eq!(max, t.values.len());
		t.updateMax(altMax);
		assert_eq!(altMax, t.values.len());
		t.updateMax(max);
		assert_eq!(altMax, t.values.len());
	}
}
