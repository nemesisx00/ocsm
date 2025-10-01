use serde::{Deserialize, Serialize};

#[derive(Clone, Copy, Debug, Default, Deserialize, PartialEq, Serialize)]
pub struct StateValue
{
	pub one: u32,
	pub two: u32,
	pub three: u32,
}

impl From<u32> for StateValue
{
	fn from(value: u32) -> Self
	{
		return Self
		{
			one: value,
			..Default::default()
		};
	}
}

impl StateValue
{
	pub fn truncate(&mut self, max: u32)
	{
		self.truncateMin(max, 0);
	}
	
	pub fn truncateMin(&mut self, max: u32, min: u32)
	{
		if self.three > max
		{
			self.three = max;
			self.two = 0;
			self.one = 0;
		}
		else if self.three + self.two > max
		{
			self.two = max - self.three;
			self.one = 0;
		}
		else if self.three + self.two + self.one > max
		{
			self.one = max - self.three - self.two;
		}
		
		if self.one < min
		{
			self.one = min;
		}
	}
}


#[cfg(test)]
mod tests
{
	#[allow(unused_imports)]
	use super::*;
	
	#[test]
	fn testTruncateMin()
	{
		let expected = StateValue { one: 2, ..Default::default() };
		let mut value = StateValue::default();
		value.truncateMin(5, 2);
		
		assert_eq!(expected, value);
	}
	
	#[test]
	fn testTruncate()
	{
		let max = 5;
		let aboveMax = 6;
		
		let mut expected = StateValue { one: 1, two: 2, three: 2 };
		let mut value = StateValue { one: aboveMax, two: 2, three: 2 };
		value.truncate(max);
		
		assert_eq!(expected, value);
		
		expected = StateValue { one: 0, two: 3, three: 2 };
		value = StateValue { one: aboveMax, two: aboveMax, three: 2 };
		value.truncate(max);
		
		assert_eq!(expected, value);
		
		expected = StateValue { one: 0, two: 0, three: 5 };
		value = StateValue { one: aboveMax, two: aboveMax, three: aboveMax };
		value.truncate(max);
		
		assert_eq!(expected, value);
	}
}
