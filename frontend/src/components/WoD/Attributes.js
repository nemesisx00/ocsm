import './Attributes.css'
import React from 'react'
import Tracker from './Tracker'
import { compareStrings, normalizeClassNames } from '../core/Utilities'

const MentalAttributes = Object.freeze({
	Intelligence: 'intelligence',
	Resolve: 'resolve',
	Wits: 'wits'
})

const PhysicalAttributes = Object.freeze({
	Dexterity: 'dexterity',
	Stamina: 'stamina',
	Strength: 'strength'
})

const SocialAttributes = Object.freeze({
	Composure: 'composure',
	Manipulation: 'manipulation',
	Presence: 'presence'
})

class Attributes extends React.Component
{
	render()
	{
		let trackers = this.generateTrackers()
		
		return (
			<div className="attributesWrapper">
				<div className="attributesLabel">Attributes</div>
				<div className="attributes">
					<div className="column">
						{trackers[Attributes.Mental.Intelligence]}
						{trackers[Attributes.Mental.Wits]}
						{trackers[Attributes.Mental.Resolve]}
					</div>
					<div className="column">
						{trackers[Attributes.Physical.Strength]}
						{trackers[Attributes.Physical.Dexterity]}
						{trackers[Attributes.Physical.Stamina]}
					</div>
					<div className="column">
						{trackers[Attributes.Social.Presence]}
						{trackers[Attributes.Social.Manipulation]}
						{trackers[Attributes.Social.Composure]}
					</div>
				</div>
			</div>
		)
	}
	
	generateTrackers()
	{
		let trackers = {}
		Object.entries(Attributes.All).forEach(entry => {
			let [upper, lower] = entry
			trackers[lower] = (<Tracker keyWord={lower} className={normalizeClassNames(`${this.props.max > 5 ? 'extended' : ''} dots ${lower}`)} label={`${upper}:`} type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes[lower]} changeHandler={(arg) => this.props.changeHandler(arg, lower)} />)
		})
		
		return trackers
	}
}

let allAttributes = Object.assign({}, MentalAttributes, PhysicalAttributes, SocialAttributes)
Object.entries(allAttributes).sort((a, b) => compareStrings(a.value, b.value))

Attributes.All = Object.freeze(allAttributes)
Attributes.Mental = MentalAttributes
Attributes.Physical = PhysicalAttributes
Attributes.Social = SocialAttributes

export default Attributes
