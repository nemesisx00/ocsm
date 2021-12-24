import '../Sheet.css'
import './MageTheAwakening.css'
import { invoke } from '@tauri-apps/api/tauri'
import { listen } from '@tauri-apps/api/event'
import React from 'react'
import Checker from '../../core/Checker'
import Tracker from '../Tracker'
import Attributes from '../Attributes'
import Merits from '../Merits'
import Skills from '../Skills'
import { compareStrings } from '../../core/Utilities'
import Details from './Details'
import Arcana from './Arcana'
import Rotes from './Rotes'

const EmptySheet = {
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
}

const DefaultHealthValue = 3
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

export default class MageTheAwakening extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = Object.assign({}, EmptySheet)
		
		this.sortRotes()
		
		this.unlisteners = {
			clearSheet: null,
			loadSheet: null,
			doSaveSheet: null
		}
	}
	
	componentDidMount()
	{
		this.unlisteners.clearSheet = listen('clearSheet', () => this.clearSheetHandler())
		this.unlisteners.loadSheet = listen('loadSheet', (obj) => this.loadSheetHandler(obj))
		this.unlisteners.doSaveSheet = listen('doSaveSheet', () => this.doSaveSheetHandler())
	}
	
	render()
	{
		//TODO: Remove this console.log call when it is no longer necessary
		console.log(this.state)
		return (
			<div className="sheet mageTheAwakening">
				<div className="column spacedOut">
					<div className="row">
						<div className="column spacedOut">
							<Details details={this.state.details} changeHandler={(obj) => this.detailsChangeHandler(obj)} />
							<Attributes attributes={this.state.attributes} max={this.state.trackers.gnosis > 5 ? this.state.trackers.gnosis : 5} changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
							<hr />
							<Skills skills={this.state.skills} max={this.state.trackers.gnosis > 5 ? this.state.trackers.gnosis : 5} changeHandler={(value, skill) => this.skillChangeHandler(value, skill)} />
						</div>
						<div className="column right trackers">
							<Tracker keyWord="health" label="Health" className="healthTracker" type={Tracker.Types.Multi} max={DefaultHealthValue + this.state.attributes.stamina} values={[this.state.trackers.damage.superficial, this.state.trackers.damage.lethal, this.state.trackers.damage.aggravated]} changeHandler={(lineStatus) => this.healthChangeHandler(lineStatus)} />
							<Tracker keyWord="willpower" label="Willpower" className="willpowerTracker" type={Tracker.Types.Single} max={this.state.attributes.composure + this.state.attributes.resolve} spent={this.state.trackers.willpowerSpent} changeHandler={(value) => this.willpowerChangeHandler(value)} />
							<Tracker keyWord="mana" label="Mana" className="manaTracker" type={Tracker.Types.Single} max={ManaGnosisScale[this.state.trackers.gnosis]} spent={this.state.trackers.manaSpent} changeHandler={(value) => this.manaChangeHandler(value)} />
							<Tracker keyWord="gnosis" label="Gnosis" className="gnosisTracker" type={Tracker.Types.Circle} max="10" value={this.state.trackers.gnosis} changeHandler={(value) => this.gnosisChangeHandler(value)} />
							<Tracker keyWord="wisdom" label="Wisdom" className="wisdomTracker" type={Tracker.Types.Circle} max="10" value={this.state.trackers.wisdom} changeHandler={(value) => this.wisdomChangeHandler(value)} />
							<hr />
							<Arcana buttonLabel="New Arcana" arcana={this.state.arcana} max={this.state.trackers.gnosis > 5 ? this.state.trackers.gnosis : 5} title="Arcana" changeHandler={(index, key, value) => this.arcanaChangeHandler(index, key, value)} />
						</div>
					</div>
					<hr />
					<Merits buttonLabel="New Merit" merits={this.state.merits} max="5" title="Merits" changeHandler={(index, key, value) => this.meritChangeHandler(index, key, value)} />
					<hr />
					<Rotes buttonLabel="New Rote" rotes={this.state.rotes} max="5" title="Rotes" changeHandler={(index, key, value) => this.roteChangeHandler(index, key, value)} />
				</div>
			</div>
		)
	}
	
	sortRotes()
	{
		this.state.rotes.sort((a, b) => {
			if(a.level < 1 && b.level > 0)
				return 1
			if(a.level > 0 && b.level < 1)
				return -1
			if(a.level > b.level)
				return 1
			if(a.level < b.level)
				return -1
			
			return compareStrings(a.spell, b.spell)
		})
	}
	
	//Event Handlers --------------------------------------------------
	
	arcanaChangeHandler(index, key, value)
	{
		let newState = {
			arcana: [...this.state.arcana]
		}
		
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
			if(!finalState.arcana.find(arcanum => arcanum.type == obj.type))
				finalState.arcana.push(obj)
		})
		
		this.setState(() => { return finalState })
	}
	
	attributeChangeHandler(value, attribute)
	{
		let newState = {
			attributes: {...this.state.attributes}
		}
		newState.attributes[attribute] = value === newState.attributes[attribute] ? value - 1 : value
		if(newState.attributes[attribute] < 1)
			newState.attributes[attribute] = 1
		this.setState(() => { return newState })
	}
	
	detailsChangeHandler(obj)
	{
		let newState = {
			details: {...this.state.details}
		}
		Object.keys(obj).forEach(key => {
			if(Object.keys(newState.details).indexOf(key) > -1)
				newState.details[key] = obj[key]
		})
		this.setState(() => { return newState })
	}
	
	gnosisChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.gnosis = value === this.state.trackers.gnosis ? value - 1 : value
		if(newState.trackers.gnosis < 1)
			newState.trackers.gnosis = 1
		if(newState.trackers.manaSpent > ManaGnosisScale[newState.trackers.gnosis])
			newState.trackers.manaSpent = ManaGnosisScale[newState.trackers.gnosis]
		this.setState(() => { return newState })
	}
	
	healthChangeHandler(lineStatus)
	{
		let newValue = {
			superficial: this.state.trackers.damage.superficial,
			lethal: this.state.trackers.damage.lethal,
			aggravated: this.state.trackers.damage.aggravated
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
		
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.damage = newValue
		this.setState(() => { return newState })
	}
	
	manaChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.manaSpent = value === this.state.trackers.manaSpent ? value - 1 : value
		
		//Sanity check
		if(newState.trackers.manaSpent > ManaGnosisScale[newState.trackers.gnosis])
			newState.trackers.manaSpent = ManaGnosisScale[newState.trackers.gnosis]
		
		this.setState(() => { return newState })
	}
	
	meritChangeHandler(index, key, value)
	{
		let newState = {
			merits: [...this.state.merits]
		}
		
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
		
		this.setState(() => { return newState })
	}
	
	roteChangeHandler(index, key, value)
	{
		let newState = {
			rotes: [...this.state.rotes]
		}
		
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
		
		this.setState(() => { return newState })
	}
	
	skillChangeHandler(value, skill)
	{
		let newState = {
			skills: {...this.state.skills}
		}
		newState.skills[skill] = value === newState.skills[skill] ? value - 1 : value
		this.setState(() => { return newState })
	}
	
	willpowerChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.willpowerSpent = value === this.state.trackers.willpowerSpent ? value - 1 : value
		this.setState(() => { return newState })
	}
	
	wisdomChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.wisdom = value === this.state.trackers.wisdom ? value - 1 : value
		if(newState.trackers.wisdom < 1)
			newState.trackers.wisdom = 1
		this.setState(() => { return newState })
	}
	
	// Backend Event Handlers
	clearSheetHandler()
	{
		this.setState(() => Object.assign({}, EmptySheet))
		this.sortRotes()
	}
	
	loadSheetHandler(obj)
	{
		if(obj && obj.payload)
		{
			let newState = null
			try { newState = JSON.parse(obj.payload) }
			catch(err) { console.log(`Failed to parse the loaded sheet: ${err}`) }
			
			if(newState !== null)
			{
				this.setState(() => { return newState })
				this.sortRotes()
			}
		}
	}
	
	doSaveSheetHandler()
	{
		console.log('saveSheet event caught!')
		invoke('SaveSheet', { state: JSON.stringify(this.state) })
	}
}
