import '../Sheet.css'
import './MageTheAwakening.css'
import { listen } from '@tauri-apps/api/event'
import React from 'react'
import Checker from '../../core/Checker'
import Tracker from '../Tracker'
import Attributes from '../Attributes'
import Merits from '../Merits'
import Skills from '../Skills'
import Details from './Details'

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
	arcana: {
		death: 0,
		fate: 0,
		forces: 0,
		life: 0,
		matter: 0,
		mind: 0,
		prime: 0,
		spirit: 0,
		space: 0,
		time: 0
	},
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
		willpowerSpent: 0
	}
}

const DefaultHealthValue = 3

export default class MageTheAwakening extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = Object.assign({}, EmptySheet)
		
		this.unlisteners = {
			clearSheet: null,
			loadSheet: null
		}
	}
	
	componentDidMount()
	{
		this.unlisteners.clearSheet = listen('clearSheet', () => this.clearSheetHandler())
		this.unlisteners.loadSheet = listen('loadSheet', (obj) => this.loadSheetHandler(obj))
	}
	
	render()
	{
		//TODO: Remove this console.log call when it is no longer necessary
		console.log(this.state)
		return (
			<div className="sheet mageTheAwakening">
				<div className="column">
					<Details details={this.state.details} changeHandler={(obj) => this.detailsChangeHandler(obj)} />
					<Attributes attributes={this.state.attributes} max={this.state.trackers.gnosis > 5 ? this.state.trackers.gnosis : 5} changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
					<hr />
					<Skills skills={this.state.skills} max={this.state.trackers.gnosis > 5 ? this.state.trackers.gnosis : 5} changeHandler={(value, skill) => this.skillChangeHandler(value, skill)} />
					<hr />
					<Merits buttonLabel="New Merit" merits={this.state.merits} max="5" title="Merits" changeHandler={(index, key, value) => this.meritChangeHandler(index, key, value)} />
				</div>
				<div className="column right">
					<div className="trackers">
						<Tracker keyWord="health" label="Health" className="healthTracker" type={Tracker.Types.Multi} max={DefaultHealthValue + this.state.attributes.stamina} values={[this.state.trackers.damage.superficial, this.state.trackers.damage.lethal, this.state.trackers.damage.aggravated]} changeHandler={(lineStatus) => this.healthChangeHandler(lineStatus)} />
						<Tracker keyWord="willpower" label="Willpower" className="willpowerTracker" type={Tracker.Types.Single} max={this.state.attributes.composure + this.state.attributes.resolve} spent={this.state.trackers.willpowerSpent} changeHandler={(value) => this.willpowerChangeHandler(value)} />
						<Tracker keyWord="mana" label="Mana" className="manaTracker" type={Tracker.Types.Single} max="10" spent={this.state.trackers.manaSpent} changeHandler={(value) => this.manaChangeHandler(value)} />
						<Tracker keyWord="gnosis" label="Gnosis" className="gnosisTracker" type={Tracker.Types.Circle} max="10" value={this.state.trackers.gnosis} changeHandler={(value) => this.gnosisChangeHandler(value)} />
					</div>
				</div>
			</div>
		)
	}
	
	//Event Handlers --------------------------------------------------
	
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
	
	clearSheetHandler()
	{
		this.setState(() => Object.assign({}, EmptySheet))
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
	
	manaChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.manaSpent = value === this.state.trackers.manaSpent ? value - 1 : value
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
}
