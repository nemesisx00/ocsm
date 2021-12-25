import './Rotes.css'
import React from 'react'
import EntryList from '../../core/EntryList'
import Tracker from '../Tracker'
import Skills from '../Skills'
import { compareStrings } from '../../core/Utilities'
import Arcana from './Arcana'

const RoteKeys = Object.freeze({
	New: 'new',
	Arcanum: 'arcanum',
	Creator: 'creator',
	Level: 'level',
	Skill: 'skill',
	Spell: 'spell'
})

class Rote extends React.Component
{
	render()
	{
		return (<div className="entry rote">
			<label>Spell:</label>
			<input name="spell" type="text" className="spell" value={this.props.spell} onChange={(event) => this.props.changeHandler(this.props.index, RoteKeys.Spell, event.target.value)} />
			<select name="arcanum" className="arcanum" value={this.props.arcanum} onChange={(event) => this.props.changeHandler(this.props.index, RoteKeys.Arcanum, event.target.value)}>
				<option value="">&nbsp;</option>
				{this.generateArcanaOptions()}
			</select>
			<Tracker keyWord={this.props.arcanum} className="dots long level" type={Tracker.Types.Circle} max={this.props.max} value={this.props.level} changeHandler={(value) => this.props.changeHandler(this.props.index, RoteKeys.Level, value)} />
			<select name="roteSkill" className="skill" value={this.props.skill} onChange={(event) => this.props.changeHandler(this.props.index, RoteKeys.Skill, event.target.value)}>
				<option value="">&nbsp;</option>
				{this.generateSkillOptions()}
			</select>
			<label>Creator:</label>
			<input name="creator" type="text" className="creator" value={this.props.creator} onChange={(event) => this.props.changeHandler(this.props.index, RoteKeys.Creator, event.target.value)} />
		</div>)
	}
	
	generateArcanaOptions()
	{
		let out = []
		Object.entries(Arcana.Types)
			.sort((a, b) => compareStrings(a[1], b[1]))
			.forEach((entry, i) => {
				let [label, value] = entry
				out.push((<option key={`arcanum-${i}`} value={value}>{label}</option>))
			})
		
		return out
	}
	
	generateSkillOptions()
	{
		let out = []
		Object.entries(Skills.All)
			.sort((a, b) => compareStrings(a[1], b[1]))
			.forEach((entry, i) => {
				let [label, value] = entry
				out.push((<option key={`skill-${i}`} value={value}>{label}</option>))
			})
		
		return out
	}
}

class Rotes extends EntryList
{
	generateEntries()
	{
		let entries = []
		
		this.props.rotes.forEach((rote, i) => {
			entries.push(<Rote key={`rote-${i}`} index={i} arcanum={rote.arcanum} creator={rote.creator} level={rote.level} skill={rote.skill} spell={rote.spell} max={this.props.max} changeHandler={(index, key, value) => this.props.changeHandler(index, key, value)} />)
		})
		
		return entries
	}
	
	addNewEntry()
	{
		this.props.changeHandler(RoteKeys.New, RoteKeys.Label, '')
	}
}

Rotes.Keys = RoteKeys

export default Rotes
