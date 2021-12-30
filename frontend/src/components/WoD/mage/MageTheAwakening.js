import '../Sheet.css'
import './MageTheAwakening.css'
import { listen } from '@tauri-apps/api/event'
import React from 'react'
import Checker from '../../core/Checker'
import Tracker from '../Tracker'
import Attributes from '../Attributes'
import Merits from '../Merits'
import Skills from '../Skills'
import ActiveSpells from './ActiveSpells'
import Arcana from './Arcana'
import Details from './Details'
import Rotes from './Rotes'
import Praxes from './Praxes'
import Advantages from './Advantages'

const EmptySheet = Object.freeze({
	attributes: {
		composure: 1,
		dexterity: 1,
		intelligence: 1,
		manipulation: 1,
		presence: 1,
		resolve: 1,
		stamina: 1,
		strength: 1,
		wits: 1
	},
	activeSpells: [
		''
	],
	advantages: {
		size: 5,
		armor: 0,
		beats: 0,
		experience: 0,
		arcaneBeats: 0,
		arcaneExperience: 0
	},
	arcana: [
		{ type: '', value: 0 }
	],
	details: {
		chronicle: '',
		concept: '',
		legacy: '',
		order: '',
		path: '',
		player: '',
		shadowName: '',
		vice: '',
		virtue: ''
	},
	merits: [
		{ label: '', value: 0 }
	],
	praxes: [
		{ arcanum: '', level: 0, spell: '' }
	],
	rotes: [
		{ arcanum: '', level: 0, spell: '', creator: '', skill: '' }
	],
	skills: {
		academics: 0,
		animalKen: 0,
		athletics: 0,
		brawl: 0,
		computer: 0,
		crafts: 0,
		drive: 0,
		empathy: 0,
		expression: 0,
		firearms: 0,
		intimidation: 0,
		investigation: 0,
		larceny: 0,
		medicine: 0,
		occult: 0,
		persuasion: 0,
		politics: 0,
		science: 0,
		socialize: 0,
		stealth: 0,
		streetwise: 0,
		subterfuge: 0,
		survival: 0,
		weaponry: 0
	},
	trackers: {
		damage: {
			superficial: 0,
			lethal: 0,
			aggravated: 0
		},
		gnosis: 1,
		manaSpent: 0,
		willpowerSpent: 0,
		wisdom: 7
	}
})

const ManaGnosisScale = Object.freeze({
	1: 10,
	2: 11,
	3: 12,
	4: 13,
	5: 15,
	6: 20,
	7: 25,
	8: 30,
	9: 50,
	10: 100
})

const SortState = (state) => {
	let sortedState = {...state}
	
	sortedState.arcana.sort(Arcana.SortArcana)
	sortedState.merits.sort(Merits.SortMerits)
	sortedState.rotes.sort(Rotes.SortRotes)
	sortedState.praxes.sort(Praxes.SortPraxes)
	
	return sortedState
}

class MageTheAwakening extends React.Component
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
			<div className="sheet mageTheAwakening">
				<div className="column">
					<div className="row">
						<div className="column">
							<Details details={this.props.sheetState.details} changeHandler={(obj) => this.detailsChangeHandler(obj)} />
						</div>
						<div className="column">
							<Advantages advantages={this.props.sheetState.advantages} attributes={this.props.sheetState.attributes} skills={this.props.sheetState.skills} changeHandler={(key, value) => this.advantagesChangeHandler(key, value)} />
						</div>
						<div className="column arcana">
							<Arcana buttonLabel="New Arcana" arcana={this.props.sheetState.arcana} max={this.props.sheetState.trackers.gnosis > 5 ? this.props.sheetState.trackers.gnosis : 5} title="Arcana" changeHandler={(index, key, value) => this.arcanaChangeHandler(index, key, value)} />
						</div>
						<div className="column trackers">
							<Tracker keyWord="health" label="Health" className="healthTracker" type={Tracker.Types.Multi} max={this.props.sheetState.advantages.size + this.props.sheetState.attributes.stamina} values={[this.props.sheetState.trackers.damage.superficial, this.props.sheetState.trackers.damage.lethal, this.props.sheetState.trackers.damage.aggravated]} changeHandler={(lineStatus) => this.healthChangeHandler(lineStatus)} />
							<Tracker keyWord="willpower" label="Willpower" className="willpowerTracker" type={Tracker.Types.Single} max={this.props.sheetState.attributes.composure + this.props.sheetState.attributes.resolve} value={this.props.sheetState.trackers.willpowerSpent} changeHandler={(value) => this.willpowerChangeHandler(value)} />
							<Tracker keyWord="mana" label="Mana" className="manaTracker" type={Tracker.Types.Single} max={ManaGnosisScale[this.props.sheetState.trackers.gnosis]} value={this.props.sheetState.trackers.manaSpent} changeHandler={(value) => this.manaChangeHandler(value)} />
							<Tracker keyWord="gnosis" label="Gnosis" className="gnosisTracker" type={Tracker.Types.Circle} max="10" value={this.props.sheetState.trackers.gnosis} changeHandler={(value) => this.gnosisChangeHandler(value)} />
							<Tracker keyWord="wisdom" label="Wisdom" className="wisdomTracker" type={Tracker.Types.Circle} max="10" value={this.props.sheetState.trackers.wisdom} changeHandler={(value) => this.wisdomChangeHandler(value)} />
						</div>
					</div>
					<hr />
					<Attributes attributes={this.props.sheetState.attributes} max={this.props.sheetState.trackers.gnosis > 5 ? this.props.sheetState.trackers.gnosis : 5} changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
					<hr />
					<Skills skills={this.props.sheetState.skills} max={this.props.sheetState.trackers.gnosis > 5 ? this.props.sheetState.trackers.gnosis : 5} changeHandler={(value, skill) => this.skillChangeHandler(value, skill)} />
					<hr />
					<Merits className="merits" buttonLabel="New Merit" merits={this.props.sheetState.merits} max="5" title="Merits" changeHandler={(index, key, value) => this.meritChangeHandler(index, key, value)} />
					<hr />
					<div className="row">
						<Rotes className="rotes" buttonLabel="New Rote" rotes={this.props.sheetState.rotes} max="5" title="Rotes" changeHandler={(index, key, value) => this.roteChangeHandler(index, key, value)} />
						<Praxes className="praxes" buttonLabel="New Praxis" title="Praxes" praxes={this.props.sheetState.praxes} max="5" changeHandler={(index, key, value) => this.praxesChangeHandler(index, key, value)} />
					</div>
					<hr />
					<div className="row">
						<ActiveSpells title="Active Spells" activeSpells={this.props.sheetState.activeSpells} changeHandler={(index, value) => this.activeSpellChangeHandler(index, value)} />
						
					</div>
				</div>
			</div>
		)
	}
	
	// Event Handlers -------------------------------------------------
	
	activeSpellChangeHandler(index, value)
	{
		let newState = {...this.props.sheetState}
		newState.activeSpells[index] = value
		this.props.updateSheetState(newState)
	}
	
	advantagesChangeHandler(key, value)
	{
		let newState = {...this.props.sheetState}
		
		console.log({key, value})
		console.log({totalHealth: this.props.sheetState.advantages.size + this.props.sheetState.attributes.stamina, size: this.props.sheetState.advantages.size, stamina: this.props.sheetState.attributes.stamina})
		
		
		let num = Number.parseInt(value)
		
		switch(key)
		{
			case 'beats':
				if(num === 5)
				{
					newState.advantages['experience'] += 1
					newState.advantages[key] = 0
				}
				else
					newState.advantages[key] = isNaN(num) ? value : num
				break
			case 'arcaneBeats':
				if(num === 5)
				{
					newState.advantages['arcaneExperience'] += 1
					newState.advantages[key] = 0
				}
				else
					newState.advantages[key] = isNaN(num) ? value : num
				break
			default:
				newState.advantages[key] = isNaN(num) ? value : num
		}
		
		this.props.updateSheetState(newState)
	}
	
	arcanaChangeHandler(index, key, value)
	{
		let newState = {...this.props.sheetState}
		
		if(Arcana.Keys.New === index)
		{
			newState.arcana.push({
				type:  Arcana.Keys.Type === key ? value : '',
				value: Arcana.Keys.Value === key ? value : 0
			})
		}
		else if(newState.arcana.length > index)
		{
			switch(key)
			{
				case Arcana.Keys.Value:
					newState.arcana[index].value = value === newState.arcana[index].value ? value - 1 : value
					break
				case Arcana.Keys.Type:
					newState.arcana[index].type = value
					break
				default:
					break
			}
		}
		
		let finalState = {
			arcana: []
		}
		newState.arcana.forEach(obj => {
			if(!finalState.arcana.find(arcanum => arcanum.type === obj.type))
				finalState.arcana.push(obj)
		})
		
		this.props.updateSheetState(newState)
	}
	
	attributeChangeHandler(value, attribute)
	{
		let newState = {...this.props.sheetState}
		
		newState.attributes[attribute] = value === newState.attributes[attribute] ? value - 1 : value
		if(newState.attributes[attribute] < 1)
			newState.attributes[attribute] = 1
		
		this.props.updateSheetState(newState)
	}
	
	detailsChangeHandler(obj)
	{
		let newState = {...this.props.sheetState}
		
		Object.keys(obj).forEach(key => {
			if(Object.keys(newState.details).indexOf(key) > -1)
				newState.details[key] = obj[key]
		})
		
		this.props.updateSheetState(newState)
	}
	
	gnosisChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.gnosis = value === this.props.sheetState.trackers.gnosis ? value - 1 : value
		if(newState.trackers.gnosis < 1)
			newState.trackers.gnosis = 1
		if(newState.trackers.manaSpent > ManaGnosisScale[newState.trackers.gnosis])
			newState.trackers.manaSpent = ManaGnosisScale[newState.trackers.gnosis]
	
		if(newState.activeSpells.length > newState.trackers.gnosis)
		{
			let newActiveSpells = []
			newState.activeSpells.forEach((spell, i) => {
				if(i < newState.trackers.gnosis)
					newActiveSpells.push(spell)
			})
			
			newState.activeSpells = newActiveSpells
		}
		else if(newState.activeSpells.length < newState.trackers.gnosis)
		{
			let newActiveSpells = [...newState.activeSpells]
			for(let i = 0; i < newState.trackers.gnosis - newState.activeSpells.length; i++)
			{
				newActiveSpells.push('')
			}
			
			newState.activeSpells = newActiveSpells
		}
		
		this.props.updateSheetState(newState)
	}
	
	healthChangeHandler(lineStatus)
	{
		let newState = {...this.props.sheetState}
		
		let newValue = {
			superficial: this.props.sheetState.trackers.damage.superficial,
			lethal: this.props.sheetState.trackers.damage.lethal,
			aggravated: this.props.sheetState.trackers.damage.aggravated
		}
		
		switch(lineStatus)
		{
			case Checker.LineStatus.Single:
				newValue.superficial--
				newValue.lethal++
				break
			case Checker.LineStatus.Double:
				newValue.lethal--
				newValue.aggravated++
				break
			case Checker.LineStatus.Triple:
				newValue.aggravated--
				break
			default:
				newValue.superficial++
		}
		
		newState.trackers.damage = newValue
		
		this.props.updateSheetState(newState)
	}
	
	manaChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.manaSpent = value === this.props.sheetState.trackers.manaSpent ? value - 1 : value
		
		//Sanity check
		if(newState.trackers.manaSpent > ManaGnosisScale[newState.trackers.gnosis])
			newState.trackers.manaSpent = ManaGnosisScale[newState.trackers.gnosis]
		
		this.props.updateSheetState(newState)
	}
	
	meritChangeHandler(index, key, value)
	{
		let newState = {...this.props.sheetState}
		
		if(Merits.Keys.New === index)
		{
			newState.merits.push({
				label:  Merits.Keys.Label === key ? value : '',
				value: Merits.Keys.Value === key ? value : 0
			})
		}
		else if(newState.merits.length > index)
		{
			switch(key)
			{
				case Merits.Keys.Label:
					newState.merits[index].label = value
					break
				case Merits.Keys.Value:
					newState.merits[index].value = value === newState.merits[index].value ? value - 1 : value
					break
				default:
					break
			}
		}
		newState.merits.sort(Merits.SortMerits)
		
		this.props.updateSheetState(newState)
	}
	
	roteChangeHandler(index, key, value)
	{
		let newState = {...this.props.sheetState}
		
		if(Rotes.Keys.New === index)
		{
			newState.rotes.push({
				arcanum:  Rotes.Keys.Arcanum === key ? value : '',
				level: Rotes.Keys.Level === key ? value : 0,
				spell: Rotes.Keys.Spell === key ? value : '',
				creator: Rotes.Keys.Creator === key ? value : '',
				skill: Rotes.Keys.Skill === key ? value : ''
			})
		}
		else if(newState.rotes.length > index)
		{
			switch(key)
			{
				case Rotes.Keys.Arcanum:
					newState.rotes[index].arcanum = value
					break
				case Rotes.Keys.Creator:
					newState.rotes[index].creator = value
					break
				case Rotes.Keys.Level:
					newState.rotes[index].level = value === newState.rotes[index].level ? value - 1 : value
					break
				case Rotes.Keys.Skill:
					newState.rotes[index].skill = value
					break
				case Rotes.Keys.Spell:
					newState.rotes[index].spell = value
					break
				default:
					break
			}
		}
		
		newState.rotes.sort(Rotes.SortRotes)
		
		this.props.updateSheetState(newState)
	}
	
	praxesChangeHandler(index, key, value)
	{
		let newState = {...this.props.sheetState}
		
		if(Praxes.Keys.New === index)
		{
			newState.praxes.push({
				arcanum:  Praxes.Keys.Arcanum === key ? value : '',
				level: Praxes.Keys.Level === key ? value : 0,
				spell: Praxes.Keys.Spell === key ? value : ''
			})
		}
		else if(newState.praxes.length > index)
		{
			switch(key)
			{
				case Praxes.Keys.Arcanum:
					newState.praxes[index].arcanum = value
					break
				case Praxes.Keys.Level:
					newState.praxes[index].level = value === newState.praxes[index].level ? value - 1 : value
					break
				case Praxes.Keys.Spell:
					newState.praxes[index].spell = value
					break
				default:
					break
			}
		}
		
		newState.praxes.sort(Praxes.SortPraxes)
		this.props.updateSheetState(newState)
	}
	
	skillChangeHandler(value, skill)
	{
		let newState = {...this.props.sheetState}
		
		newState.skills[skill] = value === this.props.sheetState.skills[skill] ? value - 1 : value
		
		this.props.updateSheetState(newState)
	}
	
	willpowerChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.willpowerSpent = value === this.props.sheetState.trackers.willpowerSpent ? value - 1 : value
		
		this.props.updateSheetState(newState)
	}
	
	wisdomChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.wisdom = value === this.props.sheetState.trackers.wisdom ? value - 1 : value
		if(newState.trackers.wisdom < 1)
			newState.trackers.wisdom = 1
		
		this.props.updateSheetState(newState)
	}
	
	// Backend Event Handlers -----------------------------------------
	
	clearSheetHandler()
	{
		let newState = Object.assign({}, EmptySheet)
		this.props.updateSheetState(newState)
	}
}

MageTheAwakening.EmptySheet = EmptySheet
MageTheAwakening.SortState = SortState

export default MageTheAwakening
