use serde::{Deserialize, Serialize};

#[derive(Clone, Copy, Debug, Default, Deserialize, Serialize)]
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
	}
}
