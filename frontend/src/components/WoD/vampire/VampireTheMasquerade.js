import './VampireTheMasquerade.css'
import React from 'react'
import Attributes from './Attributes'
import HealthTracker from '../trackers/HealthTracker'
import WillpowerTracker from '../trackers/WillpowerTracker'
import { DamageState } from '../Enums'
import { listen } from '@tauri-apps/api/event'

const DefaultHealthValue = 3;

const EmptySheet = {
	attributes: {
		strength: 1,
		dexterity: 1,
		stamina: 1,
		charisma: 1,
		manipulation: 1,
		composure: 1,
		intelligence: 1,
		wits: 1,
		resolve: 1
	},
	disciplines: [],
	details: {
		name: '',
		player: '',
		chronicle: '',
		sire: '',
		concept: '',
		ambition: '',
		desire: '',
		predator: '',
		clan: '',
		generation: ''
	},
	skills: {
		athletics: 0,
		brawl: 0,
		craft: 0,
		drive: 0,
		firearms: 0,
		larceny: 0,
		melee: 0,
		stealth: 0,
		survival: 0,
		animalKen: 0,
		etiquette: 0,
		insight: 0,
		intimidation: 0,
		leadership: 0,
		performance: 0,
		persuasion: 0,
		streetwise: 0,
		subterfuge: 0,
		academics: 0,
		awareness: 0,
		finance: 0,
		investigation: 0,
		medicine: 0,
		occult: 0,
		politics: 0,
		science: 0,
		technology: 0
	},
	trackers: {
		damage: {
			superficial: 0,
			aggravated: 0
		},
		willpowerSpent: 0
	}
}

export default class VampireTheMasquerade extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = Object.assign({}, EmptySheet)
		this.unlisteners = {}
	}
	
	componentDidMount()
	{
		this.unlisteners.loadSheet = listen('loadSheet', (obj) => this.loadSheetHandler(obj))
		this.unlisteners.newSheet = listen('newSheet', () => this.newSheetHandler())
	}
	
	render()
	{
		//TODO: Remove this console.log call when it is no longer necessary
		console.log(this.state)
		return (
			<div className="sheet changelingTheLost">
				<div className="column">
					<Attributes attributes={this.state.attributes} changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
				</div>
				<div className="column right">
					<div className="trackers">
						<HealthTracker max={DefaultHealthValue + this.state.attributes.stamina} damage={this.state.trackers.damage} changeHandler={(damageType) => this.healthChangeHandler(damageType)} />
						<WillpowerTracker max={this.state.attributes.composure + this.state.attributes.resolve} spent={this.state.trackers.willpowerSpent} changeHandler={(value) => this.willpowerChangeHandler(value)} />
					</div>
				</div>
			</div>
		)
	}
	
	//Child Component Event Handlers --------------------------------------------------
	
	attributeChangeHandler(value, attribute)
	{
		let newState = {
			attributes: {...this.state.attributes}
		}
		if(newState.attributes[attribute] === value)
			newState.attributes[attribute] = value - 1
		else
			newState.attributes[attribute] = value
		this.setState(() => { return newState })
	}
	
	healthChangeHandler(damageType)
	{
		let newValue = {
			superficial: this.state.trackers.damage.superficial,
			aggravated: this.state.trackers.damage.aggravated
		}
		
		switch(damageType)
		{
			case DamageState.Superficial:
				newValue.superficial--
				newValue.aggravated++
				break
			case DamageState.Aggravated:
				newValue.aggravated--
				break;
			default:
				newValue.superficial++
		}
		
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.damage = newValue
		this.setState(() => { return newState })
	}
	
	loadSheetHandler(obj)
	{
		if(obj && obj.payload)
		{
			let newState = null
			try { newState = JSON.parse(obj.payload) }
			catch(err) { console.log(`Failed to parse the loaded sheet: ${err}`) }
			
			if(newState !== null)
				this.setState(() => { return newState })
		}
	}
	
	newSheetHandler()
	{
		this.setState(() => Object.assign({}, EmptySheet))
	}
	
	willpowerChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.willpowerSpent = value === this.state.trackers.willpowerSpent ? value - 1 : value
		this.setState(() => { return newState })
	}
}
