#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Serialize,
	Deserialize,
};
use std::iter::Iterator;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter
};

/// The possible states for a Tracker's values.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub enum TrackerState
{
	Three,
	Two,
	One
}

/// A generic tracker for the various "Tracks" that are used
/// throughout Chronicles of Darkness.
#[derive(Clone, Debug, Default, Deserialize, PartialEq, Serialize)]
pub struct Tracker
{
	#[serde(default)]
	max: usize,
	#[serde(default)]
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
			self.values.push(state);
		}
		
		self.values.sort();
	}
	
	pub fn getMax(self) -> usize
	{
		return self.max;
	}
	
	pub fn getValue(self, index: usize) -> Option<TrackerState>
	{
		return match self.values.get(index)
		{
			Some(ts) => { Some(*ts) }
			None => { None }
		};
	}
	
	pub fn remove(&mut self, state: TrackerState)
	{
		match self.values.iter().position(|ts| *ts == state)
		{
			Some(index) => { self.values.remove(index); }
			None => {}
		}
		
		self.values.sort();
	}
	
	pub fn update(&mut self, state: TrackerState, index: usize, downgrade: bool)
	{
		match self.values.get(index)
		{
			Some(ts) =>
			{
				match ts
				{
					TrackerState::One =>
					{
						match downgrade
						{
							true => { self.remove(TrackerState::One); }
							false => { self.replace(index, TrackerState::Two); }
						}
					}
					TrackerState::Two =>
					{
						match downgrade
						{
							true => { self.replace(index, TrackerState::One); }
							false => { self.replace(index, TrackerState::Three); }
						}
					}
					TrackerState::Three =>
					{
						match downgrade
						{
							true => { self.replace(index, TrackerState::Two); }
							false => { self.remove(TrackerState::Three); }
						}
					}
				}
			}
			None =>
			{
				match downgrade
				{
					true => { self.remove(state); }
					false => { self.add(state); }
				}
			}
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
				self.values.pop();
			}
		}
	}
	
	fn replace(&mut self, index: usize, new: TrackerState)
	{
		self.values.remove(index);
		self.values.push(new);
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
	fn testUpdate()
	{
		let max = 3;
		let mut t = Tracker::new(max);
		
		t.update(TrackerState::One, 2, false);
		assert_eq!(TrackerState::One, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Two, 0, false);
		assert_eq!(TrackerState::Two, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Two, 0, true);
		assert_eq!(TrackerState::One, t.clone().getValue(0).unwrap());
		t.update(TrackerState::One, 0, true);
		assert_eq!(0, t.values.clone().len());
		t.update(TrackerState::Three, 2, false);
		assert_eq!(TrackerState::Three, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Three, 0, true);
		assert_eq!(TrackerState::Two, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Two, 0, false);
		t.update(TrackerState::Three, 0, false);
		assert_eq!(0, t.values.clone().len());
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
