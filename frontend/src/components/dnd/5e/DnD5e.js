import './DnD5e.css'
import { listen } from '@tauri-apps/api/event'
import React from 'react'
import AbilityScores from './AbilityScores'

const EmptySheet = Object.freeze({
	abilityScores: {
		charisma: null,
		constitution: null,
		dexterity: null,
		intelligence: null,
		strength: null,
		wisdom: null
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
		acrobatics: { ability: 'dexterity', proficiency: false, expertise: false },
		animalHandling: { ability: 'wisdom', proficiency: false, expertise: false },
		arcana: { ability: 'intelligence', proficiency: false, expertise: false },
		athletics: { ability: 'strength', proficiency: false, expertise: false },
		deception: { ability: 'charisma', proficiency: false, expertise: false },
		history: { ability: 'intelligence', proficiency: false, expertise: false },
		insight: { ability: 'wisdom', proficiency: false, expertise: false },
		intimidation: { ability: 'charisma', proficiency: false, expertise: false },
		investigation: { ability: 'intelligence', proficiency: false, expertise: false },
		medicine: { ability: 'wisdom', proficiency: false, expertise: false },
		nature: { ability: 'intelligence', proficiency: false, expertise: false },
		perception: { ability: 'wisdom', proficiency: false, expertise: false },
		performance: { ability: 'charisma', proficiency: false, expertise: false },
		persuasion: { ability: 'charisma', proficiency: false, expertise: false },
		religion: { ability: 'intelligence', proficiency: false, expertise: false },
		sleightOfHand: { ability: 'dexterity', proficiency: false, expertise: false },
		stealth: { ability: 'dexterity', proficiency: false, expertise: false },
		survival: { ability: 'wisdom', proficiency: false, expertise: false }
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
	return ProficiencyBonusTable.findIndex(arr => arr.indexOf(level) > -1)
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
