import React from 'react'
import Attributes from './Attributes'
import HealthTracker from './HealthTracker'
import WillpowerTracker from './WillpowerTracker'
import './ChangelingTheLost.css'

class ChangelingTheLost extends React.Component
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
					strength: 1,
					presence: 1
				},
				finesse: {
					wits: 1,
					dexterity: 1,
					manipulation: 1
				},
				resistance: {
					stamina: 1,
					composure: 1,
					resolve: 1
				}
			},
			skills: {
				mental: {},
				physical: {},
				social: {}
			}
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
			<div>
				<Attributes attributes={this.state.attributes} />
				<div className="trackers">
					<HealthTracker max={this.state.base.size + this.state.attributes.resistance.stamina} />
					<WillpowerTracker max={this.state.attributes.resistance.composure + this.state.attributes.resistance.resolve} />
				</div>
			</div>
		)
	}
}

export default ChangelingTheLost
