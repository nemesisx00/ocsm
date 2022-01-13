import './DnD5e.css'
import { listen } from '@tauri-apps/api/event'
import React from 'react'
import AbilityScores from './AbilityScores'
import Skills from './Skills'

const EmptySheet = Object.freeze({
	abilityScores: {
		charisma: '',
		constitution: '',
		dexterity: '',
		intelligence: '',
		strength: '',
		wisdom: ''
	},
	basicInfo: {
		name: {
			character: '',
			player: ''
		},
		race: null,
		background: null
	},
	classLevels: [],
	features: [],
	proficiences: [],
	skills: {
		acrobatics: Skills.Proficiency.Not,
		animalHandling: Skills.Proficiency.Not,
		arcana: Skills.Proficiency.Not,
		athletics: Skills.Proficiency.Not,
		deception: Skills.Proficiency.Not,
		history: Skills.Proficiency.Not,
		insight: Skills.Proficiency.Not,
		intimidation: Skills.Proficiency.Not,
		investigation: Skills.Proficiency.Not,
		medicine: Skills.Proficiency.Not,
		nature: Skills.Proficiency.Not,
		perception: Skills.Proficiency.Not,
		performance: Skills.Proficiency.Not,
		persuasion: Skills.Proficiency.Not,
		religion: Skills.Proficiency.Not,
		sleightOfHand: Skills.Proficiency.Not,
		stealth: Skills.Proficiency.Not,
		survival: Skills.Proficiency.Not
	},
	trackers: {
		deathSaves: { success: 0, failure: 0 },
		hitPoints: { current: 0, max: 0, temp: 0 },
		hitDice: [],
		spellSlots: []
	}
})

const ProficiencyBonusTable = Object.freeze({
	2: [1, 2, 3, 4],
	3: [5, 6, 7, 8],
	4: [9, 10, 11, 12],
	5: [13, 14, 15, 16],
	6: [17, 18, 19, 20]
})

const GetProficiencyBonusFromLevel = (level) => {
	let pb = 2
	
	Object.keys(ProficiencyBonusTable).forEach(key => {
		if(ProficiencyBonusTable[key].indexOf(level) > -1)
			pb = Number.parseInt(key)
	})
	
	return pb
}

const SortState = (state) => {
	let sortedState = {...state}
	
	return sortedState
}

class DnD5e extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.listenerHandles = {
			clearSheet: null
		}
	}
	
	componentDidMount()
	{
		this.listenerHandles.clearSheet = listen('clearSheet', () => this.clearSheetHandler())
	}
	
	render()
	{
		return (
			<div className="sheet dnd5e">
				<div className="column">
					<h1>Dungeons &amp; Dragons<br />5th Edition</h1>
					<AbilityScores abilityScores={this.props.sheetState.abilityScores} changeHandler={(ability, score) => this.abilityScoreChangeHandler(ability, score)} />
					<Skills abilityScores={this.props.sheetState.abilityScores} proficiencyBonus={GetProficiencyBonusFromLevel(this.props.sheetState.classLevels.length)} skills={this.props.sheetState.skills} changeHandler={(skill) => this.skillChangeHandler(skill)} />
				</div>
			</div>
		)
	}
	
	// Event Handlers -------------------------------------------------
	
	abilityScoreChangeHandler(ability, score)
	{
		let newState = {...this.props.sheetState}
		newState.abilityScores[ability] = score
		this.props.updateSheetState(newState)
	}
	
	skillChangeHandler(skill)
	{
		let newState = {...this.props.sheetState}
		switch(newState.skills[skill])
		{
			case Skills.Proficiency.Not:
				newState.skills[skill] = Skills.Proficiency.Half
				break
			case Skills.Proficiency.Half:
				newState.skills[skill] = Skills.Proficiency.Proficient
				break
			case Skills.Proficiency.Proficient:
				newState.skills[skill] = Skills.Proficiency.Expert
				break
			case Skills.Proficiency.Expert:
				newState.skills[skill] = Skills.Proficiency.Not
				break
			default:
				break		
		}
		this.props.updateSheetState(newState)
	}
	
	// Backend Event Handlers -----------------------------------------
	
	clearSheetHandler()
	{
		let newState = Object.assign({}, EmptySheet)
		this.props.updateSheetState(newState)
	}
}

DnD5e.EmptySheet = EmptySheet
DnD5e.GetProficiencyBonusFromLevel = GetProficiencyBonusFromLevel
DnD5e.SortState = SortState

export default DnD5e
