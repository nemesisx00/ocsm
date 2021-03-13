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
				power: {
					intelligence: 1,
					strength: 2,
					presence: 1
				},
				finesse: {
					wits: 3,
					dexterity: 4,
					manipulation: 2
				},
				resistance: {
					stamina: 2,
					composure: 3,
					resolve: 3
				}
			},
			skills: {
				mental: {
					academics: 0,
					computer: 2,
					crafts: 0,
					investigation: 3,
					medicine: 2,
					occult: 1,
					politics: 0,
					science: 0
				},
				physical: {
					athletics: 2,
					brawl: 2,
					drive: 1,
					firearms: 1,
					larceny: 2,
					stealth: 1,
					survival: 1,
					weaponry: 0
				},
				social: {
					animalKen: 2,
					empathy: 2,
					expression: 0,
					intimidation: 1,
					socialize: 0,
					streetwise: 1,
					subterfuge: 1
				}
			},
			wyrd: 5
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
					<Attributes attributes={this.state.attributes} />
					<Skills skills={this.state.skills} />
				</div>
				<div className="column right">
					<div className="trackers">
						<HealthTracker max={this.state.base.size + this.state.attributes.resistance.stamina} />
						<WillpowerTracker max={this.state.attributes.resistance.composure + this.state.attributes.resistance.resolve} />
						<GlamourTracker wyrd={this.state.wyrd} />
						<WyrdTracker wyrd={this.state.wyrd} />
					</div>
				</div>
			</div>
		)
	}
}
