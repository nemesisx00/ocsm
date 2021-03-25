import './ChangelingTheLost.css'
import React from 'react'
import Attributes from './Attributes'
import Contracts from './complex/Contracts'
import ContractDetails from './complex/ContractDetails'
import Details from './Details'
import Skills from './Skills'
import ClarityTracker from './trackers/ClarityTracker'
import GlamourTracker from './trackers/GlamourTracker'
import HealthTracker from '../trackers/HealthTracker'
import WillpowerTracker from '../trackers/WillpowerTracker'
import WyrdTracker from './trackers/WyrdTracker'
import { WyrdGlamourIntervals } from './Enums'
import { DamageState } from '../Enums'
import CreateNewContract from './complex/CreateNewContract'

const EmptyContract = {
	label: '',
	dots: 1,
	clauses: []
}

export default class ChangelingTheLost extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
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
			contracts: [
				{
					label: 'Fang and Talon (Felines)',
					dots: 1,
					clauses: [
						{
							label: 'Tongues of Birds and Words of Wolves',
							dots: 1,
							description: 'The changeling can communicate with the general type of animal represented in the Contract. This communication is partially empathic, but the changeling must either whisper to the animal in her own language or attempt to imitate whatever sounds the animal uses to express itself. Most animals make some sort of noise while responding, but they need not do so. Animals tied to the changeling by kith or this Contract instinctively feel a kinship with the changeling and readily communicate unless immediate circumstances, such as an obvious threat, intervene. Simpler, less intelligent animals communicate with less complexity. Mammals and birds are relatively easy to speak with. However, reptiles, invertebrates and most fish can provide only very simple information, such as whether or not any humans recently came near or the general location of fresh water.',
							cost: '1 Glamour',
							dicePool: 'Wyrd + Animal Ken',
							action: 'Instant',
							catch: 'The changeling gives the animal a new name.',
							resultDramatic: 'The character angers or scares the animal he tries to approach and cannot use this clause for one full scene.',
							resultFailure: 'No communication occurs.',
							resultSuccess: 'The changeling can speak to all animals of the specified type for the next scene.',
							resultExceptional: 'The animal feels affection and loyalty toward the character. The animal is actively helpful and volunteers information unasked if it considers that information important (so far as its intelligence allows).',
							modifiers: [
								{ modifier: '+1', situation: `The character imitates the animal's sounds and body language` },
								{ modifier: '-1', situation: 'The animal is frightened or hurt.' }
							]
						}
					]
				}
			],
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
	
	render()
	{
		//TODO: Remove this console.log call when it is no longer necessary
		console.log(this.state)
		return (
			<div className="sheet changelingTheLost">
				<div className="column">
					<Details details={this.state.details} changeHandler={(obj) => this.detailsChangeHandler(obj)} />
					<Attributes attributes={this.state.attributes} changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
					<Skills skills={this.state.skills} changeHandler={(value, skill) => this.skillChangeHandler(value, skill)} />
					<Contracts contracts={this.state.contracts} clickHandler={(contract) => this.contractsClickHandler(contract)} dotsHandler={(contract, value) => this.contractsDotsHandler(contract, value)} newHandler={() => this.contractsClickNewHandler()} />
				</div>
				<div className="column right">
					<div className="trackers">
						<HealthTracker max={this.state.base.size + this.state.attributes.stamina} damage={this.state.trackers.damage} changeHandler={(damageType) => this.healthChangeHandler(damageType)} />
						<WillpowerTracker max={this.state.attributes.composure + this.state.attributes.resolve} spent={this.state.trackers.willpowerSpent} changeHandler={(value) => this.willpowerChangeHandler(value)} />
						<GlamourTracker max={WyrdGlamourIntervals[this.state.trackers.wyrd]} spent={this.state.trackers.glamourSpent} changeHandler={(value) => this.glamourChangeHandler(value)} />
						<WyrdTracker wyrd={this.state.trackers.wyrd} changeHandler={(value) => this.wyrdChangeHandler(value)} />
						<ClarityTracker clarity={this.state.trackers.clarity} changeHandler={(value) => this.clarityChangeHandler(value)} />
					</div>
				</div>
				{(this.state.transient.contract !== null || this.state.transient.createNewContract === true)
					&& <div className="fullscreenOverlay" onClick={() => this.overlayCancelHandler()} />}
				{this.state.transient.createNewContract === true
					&& <CreateNewContract className="fullscreenOverlayItem" createHandler={(label) => this.contractsCreateNewHandler(label)} />}
				{this.state.transient.contract !== null && this.state.transient.createNewContract === false
					&& <ContractDetails contract={this.state.transient.contract} changeHandler={(contract, clause, key, value, modKey) => this.contractDetailsChangeHandler(contract, clause, key, value, modKey)} />}
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
	
	clarityChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.clarity = value
		this.setState(() => { return newState })
	}
	
	contractsClickHandler(contract)
	{
		//A zero dot contract wouldn't have any clauses available so don't display the ContractDetails component
		if(contract.dots > 0)
		{
			let newState = {
				transient: {...this.state.transient}
			}
			newState.transient.contract = contract
			
			this.setState(() => { return newState })
		}
	}
	
	contractsDotsHandler(contract, value)
	{
		let index = this.state.contracts.findIndex((obj) => obj.label === contract.label && obj.dots === contract.dots)
		
		let newState = {
			contracts: [...this.state.contracts]
		}
		
		if(newState.contracts[index])
		{
			if(newState.contracts[index].dots === value)
				newState.contracts[index].dots = value - 1
			else
				newState.contracts[index].dots = value
		}
		
		this.setState(() => { return newState })
	}
	
	contractsClickNewHandler()
	{
		if(this.state.transient.contract === null)
		{
			let newState = {
				transient: {...this.state.transient}
			}
			newState.transient.createNewContract = true
			
			this.setState(() => { return newState })
		}
	}
	
	contractsCreateNewHandler(label)
	{
		let newState = {
			contracts: [...this.state.contracts],
			transient: {...this.state.transient}
		}
		
		let newContract = Object.assign({}, EmptyContract)
		newContract.label = label
		newState.contracts.push(newContract)
		newState.transient.createNewContract = false
		
		this.setState(() => { return newState })
	}
	
	contractDetailsChangeHandler(contract, clause, key, value, modifierKey)
	{
		let contractIndex = this.state.contracts.findIndex((obj) => obj.label === contract.label && obj.dots === contract.dots)
		if(contractIndex > -1)
		{
			let clauseIndex = this.state.contracts[contractIndex].clauses.findIndex((obj) => obj.label === clause.label && obj.dots === clause.dots)
			
			let newState = {
				contracts: [...this.state.contracts]
			}
			
			if(newState.contracts[contractIndex])
			{
				if(newState.contracts[contractIndex].clauses[clauseIndex])
				{
					if(typeof(modifierKey) !== 'undefined' && modifierKey !== null)
						newState.contracts[contractIndex].clauses[clauseIndex].modifiers[modifierKey][key] = value
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
			
			this.setState(() => { return newState })
		}
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
	
	glamourChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.glamourSpent = value === this.state.trackers.glamourSpent ? value - 1 : value
		this.setState(() => { return newState })
	}
	
	healthChangeHandler(damageType)
	{
		let totalHealth = this.state.base.size + this.state.attributes.stamina
		let possibleDamage = 0
		let newValue = {
			superficial: this.state.trackers.damage.superficial,
			lethal: this.state.trackers.damage.lethal,
			aggravated: this.state.trackers.damage.aggravated
		}
		
		switch(damageType)
		{
			case DamageState.Superficial:
				possibleDamage = totalHealth - this.state.trackers.damage.aggravated
				if(newValue.lethal < possibleDamage)
				{
					newValue.superficial--
					newValue.lethal++
				}
				break
			case DamageState.Lethal:
				possibleDamage = totalHealth
				if(newValue.aggravated < possibleDamage)
				{
					newValue.lethal--
					newValue.aggravated++
				}
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
	
	overlayCancelHandler()
	{
		let newState = {
			transient: {...this.state.transient}
		}
		newState.transient.contract = null
		newState.transient.createNewContract = false
		
		this.setState(() => { return newState })
	}
	
	skillChangeHandler(value, skill)
	{
		let newState = {
			skills: {...this.state.skills}
		}
		if(newState.skills[skill] === value)
			newState.skills[skill] = value - 1
		else
			newState.skills[skill] = value
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
	
	wyrdChangeHandler(value)
	{
		let newState = {
			trackers: {...this.state.trackers}
		}
		newState.trackers.wyrd = value
		this.setState(() => { return newState })
	}
}
