#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		vtr2e::{
			details::{
				Details,
				DetailsField,
			}
		}
	},
};

pub static KindredDetails: AtomRef<Details> = |_| Details::default();

pub fn updateDetail<T>(scope: &Scope<T>, field: DetailsField, value: String)
{
	let details = use_atom_ref(&scope, KindredDetails);
	
	match field
	{
		DetailsField::Bloodline => { details.write().bloodline = value; }
		DetailsField::Chronicle => { details.write().chronicle = value; }
		DetailsField::Clan => { details.write().clan = value; }
		DetailsField::Concept => { details.write().concept = value; }
		DetailsField::Covenant => { details.write().covenant = value; }
		DetailsField::Dirge => { details.write().dirge = value; }
		DetailsField::Mask => { details.write().mask = value; }
		DetailsField::Name => { details.write().name = value; }
		DetailsField::Player => { details.write().player = value; }
	}
}
