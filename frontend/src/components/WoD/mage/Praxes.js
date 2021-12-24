import './Praxes.css'
import React from 'react'
import EntryList from '../../core/EntryList'
import { compareStrings } from '../../core/Utilities'
import Tracker from '../Tracker'
import Arcana from './Arcana'

const PraxesKeys = Object.freeze({
	New: 'new',
	Arcanum: 'arcanum',
	Level: 'level',
	Spell: 'spell'
})

class Praxis extends React.Component
{
	render()
	{
		return (<div className="entry praxis">
			<input type="text" className="spell" value={this.props.spell} onChange={(event) => this.props.changeHandler(this.props.index, PraxesKeys.Spell, event.target.value)} />
			<select name="arcanum" className="arcanum" value={this.props.arcanum} onChange={(event) => this.props.changeHandler(this.props.index, PraxesKeys.Arcanum, event.target.value)}>
				<option value="">&nbsp;</option>
				{this.generateArcanaOptions()}
			</select>
			<Tracker keyWord="praxis" className="dots level" type={Tracker.Types.Circle} max={this.props.max} value={this.props.level} changeHandler={(value) => this.props.changeHandler(this.props.index, PraxesKeys.Level, value)} />
		</div>)
	}
	
	generateArcanaOptions()
	{
		let out = []
		Object.entries(Arcana.Types)
			.sort((a, b) => compareStrings(a[1], b[1]))
			.forEach(entry => {
				let [label, value] = entry
				out.push((<option value={value}>{label}</option>))
			})
		
		return out
	}
}

class Praxes extends EntryList
{
	generateEntries()
	{
		let entries = []
		
		this.props.praxes.forEach((spell, i) => {
			entries.push(<Praxis key={`praxis-${i}`} index={i} arcanum={spell.arcanum} level={spell.level} spell={spell.spell} max={this.props.max} changeHandler={(index, key, value) => this.props.changeHandler(index, key, value)} />)
		})
		
		return entries
	}
	
	addNewEntry()
	{
		this.props.changeHandler(PraxesKeys.New, PraxesKeys.Label, '')
	}
}

Praxes.Keys = PraxesKeys

export default Praxes
