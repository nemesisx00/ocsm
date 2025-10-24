use serde::{Deserialize, Serialize};
use strum_macros::{AsRefStr, EnumIter};

#[derive(AsRefStr, Clone, Copy, Debug, Default, Deserialize, EnumIter, Eq, Hash, PartialEq, PartialOrd, Ord, Serialize)]
pub enum Discipline
{
	Animalism,
	Auspex,
	Celerity,
	Dominate,
	Majesty,
	Nightmare,
	Obfuscate,
	Protean,
	Resilience,
	Vigor,
	#[strum(serialize = "Coil of the Ascendant")]
	CoilAscendant,
	#[strum(serialize = "Coil of the Voivode")]
	CoilVoivode,
	#[strum(serialize = "Coil of the Wyrm")]
	CoilWrym,
	#[strum(serialize = "Crúac")]
	Cruac,
	#[strum(serialize = "Theban Sorcery")]
	ThebanSorcery,
	
	#[default]
	#[strum(serialize = "None", serialize = "")]
	None,
}

impl From<usize> for Discipline
{
	fn from(value: usize) -> Self
	{
		return match value
		{
			1 => Self::Animalism,
			2 => Self::Auspex,
			3 => Self::Celerity,
			4 => Self::Dominate,
			5 => Self::Majesty,
			6 => Self::Nightmare,
			7 => Self::Obfuscate,
			8 => Self::Protean,
			9 => Self::Resilience,
			10 => Self::Vigor,
			_ => Self::None,
		}
	}
}
