#![allow(non_snake_case, non_upper_case_globals)]

use crate::{
	cod::{
		tracks::{
			Tracker,
		},
		traits::{
			Attributes,
			Skills
		},
		vtr2e::{
			details::{
				Details,
			}
		}
	}
};

#[derive(Clone, Default)]
pub struct Kindred
{
	pub attributes: Attributes,
	pub details: Details,
	pub disciplines: Vec<usize>,
	pub health: Tracker,
	pub skills: Skills,
	pub willpower: Tracker,
}
