import './ChangelingTheLost.css'
import React from 'react'
import Attributes from './Attributes'
import GlamourTracker from './GlamourTracker'
import HealthTracker from './HealthTracker'
import Skills from './Skills'
import WillpowerTracker from './WillpowerTracker'
import WyrdTracker from './WyrdTracker'

export default class ChangelingTheLost extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			base: {
				size: 5
			},
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
			wyrd: 1
		}
		
		/*
		Notes: High weird allows attributes, skills, and contracts to go above 5 dots... I think. So that will need to be possible.
		Should keep all the attributes visible at once
		The skills can be put into a tabbed interface based on base category
		Merits and Contracts should be visible at the same time
		Flaws and Pledges aren't necessarily going to be needed at all times, so they can take a slightly lower priority in terms of the layout
		
		Merits, Contracts, Flaws, and Pledges should have a sort of "pop up" or "expandable" view mode as they all have more information associated with them than would good to show on the main page all the time
		
		
		*/
	}
	
	render()
	{
		return (
			<div className="sheet changelingTheLost">
				<div className="column">
					<Attributes attributes={this.state.attributes} changeHandler={(value, attribute) => this.attributeChangeHandler(value, attribute)} />
					<Skills skills={this.state.skills} changeHandler={(value, skill) => this.skillChangeHandler(value, skill)} />
				</div>
				<div className="column right">
					<div className="trackers">
						<HealthTracker max={this.state.base.size + this.state.attributes.stamina} />
						<WillpowerTracker max={this.state.attributes.composure + this.state.attributes.resolve} />
						<GlamourTracker wyrd={this.state.wyrd} />
						<WyrdTracker wyrd={this.state.wyrd} />
					</div>
				</div>
			</div>
		)
	}
	
	//Child Component Event Handlers
	attributeChangeHandler(value, attribute)
	{
		let newState = {
			attributes: Object.assign({}, this.state.attributes)
		}
		newState.attributes[attribute] = value
		this.setState(() => { return newState })
	}
	
	skillChangeHandler(value, skill)
	{
		let newState = {
			skills: Object.assign({}, this.state.skills)
		}
		newState.skills[skill] = value
		this.setState(() => { return newState })
	}
}
