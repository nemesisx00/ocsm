import './Skills.css'
import React from 'react'
import Tracker from './Tracker'
import { compareStrings, normalizeClassNames } from '../core/Utilities'

const MentalSkills = Object.freeze({
	Academics: 'academics',
	Computer: 'computer',
	Crafts: 'crafts',
	Investigation: 'investigation',
	Medicine: 'medicine',
	Occult: 'occult',
	Politics: 'politics',
	Science: 'science'
})

const PhysicalSkills = Object.freeze({
	Athletics: 'athletics',
	Brawl: 'brawl',
	Drive: 'drive',
	Firearms: 'firearms',
	Larceny: 'larceny',
	Stealth: 'stealth',
	Survival: 'survival',
	Weaponry: 'weaponry'
})

const SocialSkills = Object.freeze({
	'Animal Ken': 'animalKen',
	Empathy: 'empathy',
	Expression: 'expression',
	Intimidation: 'intimidation',
	Persuasion: 'persuasion',
	Socialize: 'socialize',
	Streetwise: 'streetwise',
	Subterfuge: 'subterfuge'
})

class Skills extends React.Component
{
	render()
	{
		let trackerLists = this.generateTrackerLists()
		
		return (
			<div className="skillsWrapper">
				<div className="skillsLabel">Skills</div>
				<div className="skills">
					<div className="row">
						<div className="column">
							{trackerLists.Mental}
						</div>
					</div>
					<div className="row">
						<div className="column">
							{trackerLists.Physical}
						</div>
					</div>
					<div className="row">
						<div className="column">
							{trackerLists.Social}
						</div>
					</div>
				</div>
			</div>
		)
	}
	
	generateTrackerLists()
	{
		let trackerLists = {
			Mental: [],
			Physical: [],
			Social: []
		}
		
		Object.entries(MentalSkills).forEach(entry => {
			let [upper, lower] = entry
			trackerLists.Mental.push((<Tracker keyWord={lower} className={normalizeClassNames(`${this.props.max > 5 ? 'extended' : ''} dots ${lower}`)} label={`${upper}:`} type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills[lower]} changeHandler={(arg) => this.props.changeHandler(arg, lower)} />))
		})
		
		Object.entries(PhysicalSkills).forEach(entry => {
			let [upper, lower] = entry
			trackerLists.Physical.push((<Tracker keyWord={lower} className={normalizeClassNames(`${this.props.max > 5 ? 'extended' : ''} dots ${lower}`)} label={`${upper}:`} type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills[lower]} changeHandler={(arg) => this.props.changeHandler(arg, lower)} />))
		})
		
		Object.entries(SocialSkills).forEach(entry => {
			let [upper, lower] = entry
			trackerLists.Social.push((<Tracker keyWord={lower} className={normalizeClassNames(`${this.props.max > 5 ? 'extended' : ''} dots ${lower}`)} label={`${upper}:`} type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills[lower]} changeHandler={(arg) => this.props.changeHandler(arg, lower)} />))
		})
		
		return trackerLists
	}
}

let allSkills = Object.assign({}, MentalSkills, PhysicalSkills, SocialSkills)
Object.entries(allSkills).sort((a, b) => compareStrings(a.value, b.value))

Skills.All = Object.freeze(allSkills)
Skills.Mental = MentalSkills
Skills.Physical = PhysicalSkills
Skills.Social = SocialSkills

export default Skills
