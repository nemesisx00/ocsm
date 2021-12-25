import '../Sheet.css'
import './ChangelingTheLost.css'
import { listen } from '@tauri-apps/api/event'
import React from 'react'
import Checker from '../../core/Checker'
import Tracker from '../Tracker'
import Attributes from '../Attributes'
import Contracts from './complex/Contracts'
import ContractDetails from './complex/ContractDetails'
import CreateNewContract from './complex/CreateNewContract'
import Details from './Details'
import Skills from '../Skills'

const EmptyContract = Object.freeze({
	label: '',
	dots: 1,
	clauses: []
})

const EmptyModifier = Object.freeze({
	modifier: '',
	situation: ''
})

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
	base: {
		size: 5
	},
	contracts: [],
	details: {
		name: '',
		player: '',
		chronicle: '',
		concept: '',
		virtue: '',
		vice: '',
		seeming: '',
		kith: '',
		court: ''
	},
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
		investigation: 0,
		intimidation: 0,
		larceny: 0,
		medicine: 0,
		occult: 0,
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
		clarity: 7,
		damage: {
			superficial: 0,
			lethal: 0,
			aggravated: 0
		},
		glamourSpent: 0,
		willpowerSpent: 0,
		wyrd: 1
	},
	transient: {
		contract: null,
		createNewContract: false
	}
})

const WyrdGlamourIntervals = Object.freeze({
	1: 10,
	2: 11,
	3: 12,
	4: 13,
	5: 14,
	6: 15,
	7: 20,
	8: 30,
	9: 50,
	10: 100
})

class ChangelingTheLost extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.listenerHandles = {
			clearSheet: null
		}
		
		/*
		Notes:
			High wyrd allows attributes, skills, and contracts to go above 5 dots... I think. So that will need to be possible.
			Should keep all the attributes visible at once
			The skills can be put into a tabbed interface based on base category
			Merits and Contracts should be visible at the same time
			Flaws and Pledges aren't necessarily going to be needed at all times, so they can take a slightly lower priority in terms of the layout
			
			Merits, Contracts, Flaws, and Pledges should have a sort of "pop up" or "expandable" view mode as they all have more information associated with them than would good to show on the main page all the time
		*/
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
					<Details details={this.props.sheetState.details} changeHandler={(obj) => this.detailsChangeHandler(obj)} />
					<Attributes attributes={this.props.sheetState.attributes} max="5" changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
					<Skills skills={this.props.sheetState.skills} max="5" changeHandler={(value, skill) => this.skillChangeHandler(value, skill)} />
					<Contracts contracts={this.props.sheetState.contracts} clickHandler={(contract) => this.contractsClickHandler(contract)} dotsHandler={(contract, value) => this.contractsDotsHandler(contract, value)} newHandler={() => this.contractsClickNewHandler()} />
				</div>
				<div className="column right">
					<div className="trackers">
						<Tracker keyWord="health" label="Health" type={Tracker.Types.Multi} max={this.props.sheetState.base.size + this.props.sheetState.attributes.stamina} values={[this.props.sheetState.trackers.damage.superficial, this.props.sheetState.trackers.damage.lethal, this.props.sheetState.trackers.damage.aggravated]} changeHandler={(lineStatus) => this.healthChangeHandler(lineStatus)} />
						<Tracker keyWord="willpower" label="Willpower" type={Tracker.Types.Single} max={this.props.sheetState.attributes.composure + this.props.sheetState.attributes.resolve} spent={this.props.sheetState.trackers.willpowerSpent} changeHandler={(value) => this.willpowerChangeHandler(value)} />
						<Tracker keyWord="glamour" label="Glamour" type={Tracker.Types.Single} max={WyrdGlamourIntervals[this.props.sheetState.trackers.wyrd]} spent={this.props.sheetState.trackers.glamourSpent} changeHandler={(value) => this.glamourChangeHandler(value)} />
						<Tracker keyWord="wyrd" label="Wyrd" type={Tracker.Types.Circle} max="10" value={this.props.sheetState.trackers.wyrd} changeHandler={(value) => this.wyrdChangeHandler(value)} />
						<Tracker keyWord="clarity" label="Clarity" type={Tracker.Types.Circle} max="10" value={this.props.sheetState.trackers.clarity} changeHandler={(value) => this.clarityChangeHandler(value)} />
					</div>
				</div>
				{(this.props.sheetState.transient.contract !== null || this.props.sheetState.transient.createNewContract === true)
					&& <div className="fullscreenOverlay" onClick={() => this.overlayCancelHandler()} />}
				{this.props.sheetState.transient.createNewContract === true
					&& <CreateNewContract className="fullscreenOverlayItem" createHandler={(label) => this.contractsCreateNewHandler(label)} />}
				{this.props.sheetState.transient.contract !== null && this.props.sheetState.transient.createNewContract === false
					&& <ContractDetails contract={this.props.sheetState.transient.contract} changeHandler={(contract, clause, key, value, modKey) => this.contractDetailsChangeHandler(contract, clause, key, value, modKey)} />}
			</div>
		)
	}
	
	// Event Handlers -------------------------------------------------
	
	attributeChangeHandler(value, attribute)
	{
		let newState = {...this.props.sheetState}
		
		newState.attributes[attribute] = value === newState.attributes[attribute] ? value - 1 : value
		if(newState.attributes[attribute] < 1)
			newState.attributes[attribute] = 1
		
		this.props.updateSheetState(newState)
	}
	
	clarityChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.clarity = value === newState.trackers.clarity ? value - 1 : value
		if(newState.trackers.clarity < 1)
			newState.trackers.clarity = 1
		
		this.props.updateSheetState(newState)
	}
	
	contractsClickHandler(contract)
	{
		//A zero dot contract wouldn't have any clauses available so don't display the ContractDetails component
		if(contract.dots > 0)
		{
			let newState = {...this.props.sheetState}
			
			newState.transient.contract = contract
			
			this.props.updateSheetState(newState)
		}
	}
	
	contractsDotsHandler(contract, value)
	{
		let newState = {...this.props.sheetState}
		
		let index = this.props.sheetState.contracts.findIndex((obj) => obj.label === contract.label && obj.dots === contract.dots)
		
		if(newState.contracts[index])
			newState.contracts[index].dots = value === newState.contracts[index].dots ? value - 1 : value
		
		this.props.updateSheetState(newState)
	}
	
	contractsClickNewHandler()
	{
		if(this.props.sheetState.transient.contract === null)
		{
			let newState = {...this.props.sheetState}
			
			newState.transient.createNewContract = true
			
			this.props.updateSheetState(newState)
		}
	}
	
	contractsCreateNewHandler(label)
	{
		let newState = {...this.props.sheetState}
		
		let newContract = Object.assign({}, EmptyContract)
		newContract.label = label
		newState.contracts.push(newContract)
		newState.transient.createNewContract = false
		
		this.props.updateSheetState(newState)
	}
	
	contractDetailsChangeHandler(contract, clause, key, value, modifierKey)
	{
		let contractIndex = this.props.sheetState.contracts.findIndex((obj) => obj.label === contract.label && obj.dots === contract.dots)
		if(contractIndex > -1)
		{
			let clauseIndex = this.props.sheetState.contracts[contractIndex].clauses.findIndex((obj) => obj.label === clause.label && obj.dots === clause.dots)
			
			let newState = {...this.props.sheetState}
			
			if(newState.contracts[contractIndex])
			{
				if(newState.contracts[contractIndex].clauses[clauseIndex])
				{
					if(typeof(modifierKey) !== 'undefined' && modifierKey !== null)
					{
						let currentModifiers = newState.contracts[contractIndex].clauses[clauseIndex].modifiers
						if(typeof(currentModifiers[modifierKey]) === 'undefined')
						{
							//Only add a new row if there are no existing modifiers or the last modifier is not completely empty
							let lastModifierIndex = currentModifiers.length - 1
							if(lastModifierIndex < 0 || currentModifiers[lastModifierIndex].modifier !== '' || currentModifiers[lastModifierIndex].situation !== '')
								currentModifiers.push(Object.assign({}, EmptyModifier))
						}
						else
							currentModifiers[modifierKey][key] = value
					}
					else
						newState.contracts[contractIndex].clauses[clauseIndex][key] = value
				}
				else
				{
					if(clauseIndex < 0)
					{
						clauseIndex = newState.contracts[contractIndex].clauses.length
						newState.contracts[contractIndex].clauses.push(clause)
					}
				}
			}
			
			this.props.updateSheetState(newState)
		}
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
	
	glamourChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.glamourSpent = value === this.props.sheetState.trackers.glamourSpent ? value - 1 : value
		
		this.props.updateSheetState(newState)
	}
	
	healthChangeHandler(lineStatus)
	{
		let newState = {...this.props.sheetState}
		
		let totalHealth = this.props.sheetState.base.size + this.props.sheetState.attributes.stamina
		let possibleDamage = 0
		let newValue = {
			superficial: this.props.sheetState.trackers.damage.superficial,
			lethal: this.props.sheetState.trackers.damage.lethal,
			aggravated: this.props.sheetState.trackers.damage.aggravated
		}
		
		switch(lineStatus)
		{
			case Checker.LineStatus.Single:
				possibleDamage = totalHealth - this.props.sheetState.trackers.damage.aggravated
				if(newValue.lethal < possibleDamage)
				{
					newValue.superficial--
					newValue.lethal++
				}
				break
			case Checker.LineStatus.Double:
				possibleDamage = totalHealth
				if(newValue.aggravated < possibleDamage)
				{
					newValue.lethal--
					newValue.aggravated++
				}
				break
			case Checker.LineStatus.Triple:
				newValue.aggravated--
				break;
			default:
				newValue.superficial++
		}
		
		newState.trackers.damage = newValue
		
		this.props.updateSheetState(newState)
	}
	
	overlayCancelHandler()
	{
		let newState = {...this.props.sheetState}
		
		newState.transient.contract = null
		newState.transient.createNewContract = false
		
		this.props.updateSheetState(newState)
	}
	
	skillChangeHandler(value, skill)
	{
		let newState = {...this.props.sheetState}
		
		newState.skills[skill] = value === newState.skills[skill] ? value - 1 : value
		
		this.props.updateSheetState(newState)
	}
	
	willpowerChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.willpowerSpent = value === this.props.sheetState.trackers.willpowerSpent ? value - 1 : value
		
		this.props.updateSheetState(newState)
	}
	
	wyrdChangeHandler(value)
	{
		let newState = {...this.props.sheetState}
		
		newState.trackers.wyrd = value === newState.trackers.wyrd ? value - 1 : value
		if(newState.trackers.wyrd < 1)
			newState.trackers.wyrd = 1
		if(newState.trackers.glamourSpent >= WyrdGlamourIntervals[newState.trackers.wyrd])
			newState.trackers.glamourSpent = WyrdGlamourIntervals[newState.trackers.wyrd]
		
		this.props.updateSheetState(newState)
	}
	
	// Backend Event Handlers -----------------------------------------
	
	clearSheetHandler()
	{
		let newState = Object.assign({}, EmptySheet)
		this.props.updateSheetState(newState)
	}
}

ChangelingTheLost.EmptySheet = EmptySheet

export default ChangelingTheLost
