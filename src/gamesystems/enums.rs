use serde::{Deserialize, Serialize};

#[derive(Clone, Copy, Debug, Default, Deserialize, Eq, PartialEq, PartialOrd, Ord, Serialize)]
pub enum GameSystem
{
	#[default]
	None,
	Cofd,
	Vtr2e,
}

impl From<String> for GameSystem
{
	fn from(value: String) -> Self
	{
		return match value.as_str()
		{
			"Cofd" => Self::Cofd,
			"Vtr2e" => Self::Vtr2e,
			_ => Self::None,
		};
	}
}
