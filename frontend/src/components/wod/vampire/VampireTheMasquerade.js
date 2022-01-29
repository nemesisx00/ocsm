import '../Sheet.css'
import './VampireTheMasquerade.css'
import React from 'react'
import Checker from '../../core/Checker'
import Tracker from '../Tracker'
import Attributes from './Attributes'
import { listen } from '@tauri-apps/api/event'

const DefaultHealthValue = 3;

const EmptySheet = Object.freeze({
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
})

class VampireTheMasquerade extends React.Component
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
			<div className="sheet changelingTheLost">
				<div className="column">
					<Attributes attributes={this.props.sheetState.attributes} max="5" changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
				</div>
				<div className="column right">
					<div className="trackers">
						<Tracker keyWord="health" label="Health" type={Tracker.Types.Multi} max={DefaultHealthValue + this.props.sheetState.attributes.stamina} values={[this.props.sheetState.trackers.damage.superficial, this.props.sheetState.trackers.damage.aggravated]} changeHandler={(lineStatus) => this.healthChangeHandler(lineStatus)} />
						<Tracker keyWord="willpower" label="Willpower" type={Tracker.Types.Single} max={this.props.sheetState.attributes.composure + this.props.sheetState.attributes.resolve} spent={this.props.sheetState.trackers.willpowerSpent} changeHandler={(value) => this.willpowerChangeHandler(value)} />
					</div>
				</div>
			</div>
		)
	}
	
	// Event Handlers -------------------------------------------------
	
	attributeChangeHandler(value, attribute)
	{
		let newState = {...this.props.sheetState}
		
		if(newState.attributes[attribute] === value)
			newState.attributes[attribute] = value - 1
		else
			newState.attributes[attribute] = value
		
		this.props.updateSheetState(newState)
	}
	
	healthChangeHandler(lineStatus)
	{
		let newState = {...this.props.sheetState}
		
		let newObject = {
			superficial: this.props.sheetState.trackers.damage.superficial,
			aggravated: this.props.sheetState.trackers.damage.aggravated
		}
		
		switch(lineStatus)
		{
			case Checker.LineStatus.Single:
				newObject.superficial--
				newObject.aggravated++
				break
			case Checker.LineStatus.Triple:
				newObject.aggravated--
				break
			default:
				newObject.superficial++
		}
		
		newState.trackers.damage = newObject
		
		this.props.updateSheetState(newState)
	}
	
	willpowerChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.willpowerSpent = value === this.props.sheetState.trackers.willpowerSpent ? value - 1 : value
		
		this.props.updateSheetState(newState)
	}
	
	// Backend Event Handlers -----------------------------------------
	
	clearSheetHandler()
	{
		this.setState(() => Object.assign({}, EmptySheet))
	}
}

VampireTheMasquerade.EmptySheet = EmptySheet

export default VampireTheMasquerade
