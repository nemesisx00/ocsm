use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_context,
	use_memo, use_signal, Element, GlobalSignal, Readable, Signal, Writable};
use crate::gamesystems::cofd::{AttributesMentalElement, AttributesPhysicalElement,
	AttributesSocialElement, SkillSpecialtyListElement, SkillsMentalElement,
	SkillsPhysicalElement, SkillsSocialElement};
use crate::gamesystems::cofd::data::{AttributesMental, AttributesPhysical,
	AttributesSocial, SkillsMental, SkillsPhysical, SkillsSocial, Specialty};
use crate::gamesystems::Vtr2eSheet;

#[component]
pub fn TraitsElement() -> Element
{
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	
	let attrMental: Signal<AttributesMental> = use_signal(|| sheetData().attributes.into());
	let attrPhysical: Signal<AttributesPhysical> = use_signal(|| sheetData().attributes.into());
	let attrSocial: Signal<AttributesSocial> = use_signal(|| sheetData().attributes.into());
	
	let skillMental: Signal<SkillsMental> = use_signal(|| sheetData().skills.into());
	let skillPhysical: Signal<SkillsPhysical> = use_signal(|| sheetData().skills.into());
	let skillSocial: Signal<SkillsSocial> = use_signal(|| sheetData().skills.into());
	
	let mut specialties: Signal<Vec<Specialty>> = use_signal(|| sheetData().specialties);
	
	if specialties().is_empty()
	{
		specialties.write().push(Default::default());
	}
	
	use_memo(move || {
		let mut sheetDataWrite = sheetData.write();
		
		sheetDataWrite.attributes.updateMental(attrMental());
		sheetDataWrite.attributes.updatePhysical(attrPhysical());
		sheetDataWrite.attributes.updateSocial(attrSocial());
		
		sheetDataWrite.skills.updateMental(skillMental());
		sheetDataWrite.skills.updatePhysical(skillPhysical());
		sheetDataWrite.skills.updateSocial(skillSocial());
		
		sheetDataWrite.specialties = specialties();
	});
	
	let traitMax = sheetData().calculateMaxTrait();
	
	return rsx!(
		rect
		{
			direction: "vertical",
			spacing: "10",
			width: "fill",
			
			rect
			{
				direction: "horizontal",
				main_align: "space-evenly",
				width: "fill",
				
				AttributesMentalElement { signal: attrMental, traitMax }
				AttributesPhysicalElement { signal: attrPhysical, traitMax }
				AttributesSocialElement { signal: attrSocial, traitMax }
			}
			
			rect
			{
				direction: "horizontal",
				main_align: "space-evenly",
				width: "fill",
				
				SkillsMentalElement { signal: skillMental, traitMax }
				SkillsPhysicalElement { signal: skillPhysical, traitMax }
				SkillsSocialElement { signal: skillSocial, traitMax }
			}
			
			SkillSpecialtyListElement { signal: specialties, width: "fill" }
		}
	);
}
